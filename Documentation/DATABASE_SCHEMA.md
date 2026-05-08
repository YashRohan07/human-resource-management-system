# Database Schema

This document describes the current database structure of the HRMS project.

The database is managed using Entity Framework Core migrations with SQL Server.

---

# Database Name

```text
HRMSDb
```

---

# Main Tables

## AppUsers

Stores application login users.

### Columns

| Column | Type | Notes |
|---|---|---|
| Id | int | Primary Key |
| FullName | nvarchar(150) | Required |
| Email | nvarchar(150) | Unique |
| PasswordHash | nvarchar(500) | BCrypt hashed password |
| Role | nvarchar(50) | Admin / HR |

---

## Employees

Stores employee information.

### Columns

| Column | Type | Notes |
|---|---|---|
| Id | int | Primary Key |
| FullName | nvarchar(150) | Required |
| Email | nvarchar(150) | Unique |
| Phone | nvarchar(30) | Required |
| Department | nvarchar(100) | Required |
| Position | nvarchar(100) | Required |
| AccountNumber | nvarchar(100) | Required |
| EmploymentStatus | nvarchar(50) | Active / Inactive / Terminated |
| IsDeleted | bit | Soft delete flag |
| CreatedAt | datetime2 | Auto managed |
| UpdatedAt | datetime2 | Auto managed |

---

## Salaries

Stores current employee salary information.

### Columns

| Column | Type | Notes |
|---|---|---|
| Id | int | Primary Key |
| EmployeeId | int | Foreign Key |
| BasicSalary | decimal(18,2) | Required |
| Bonus | decimal(18,2) | Default 0 |
| Deduction | decimal(18,2) | Default 0 |
| EffectiveFrom | datetime2 | Required |
| CreatedAt | datetime2 | Auto managed |
| UpdatedAt | datetime2 | Auto managed |

---

## Payrolls

Stores monthly payroll history.

Payroll values are stored as snapshots so old payroll records remain unchanged even if salary changes later.

### Columns

| Column | Type | Notes |
|---|---|---|
| Id | int | Primary Key |
| EmployeeId | int | Foreign Key |
| Month | int | Payroll month |
| Year | int | Payroll year |
| BasicSalarySnapshot | decimal(18,2) | Salary snapshot |
| BonusSnapshot | decimal(18,2) | Bonus snapshot |
| DeductionSnapshot | decimal(18,2) | Deduction snapshot |
| Tax | decimal(18,2) | Tax amount |
| GrossSalary | decimal(18,2) | Gross salary |
| NetSalary | decimal(18,2) | Final salary |
| GeneratedAt | datetime2 | Payroll generation date |
| CreatedAt | datetime2 | Auto managed |
| UpdatedAt | datetime2 | Auto managed |

---

# Relationships

## Employee → Salary

```text
One-to-One
```

One employee has one current salary record.

---

## Employee → Payroll

```text
One-to-Many
```

One employee can have many payroll records.

---

# Soft Delete

Soft delete is implemented for employees.

Instead of removing employee records permanently:

```text
IsDeleted = true
```

This helps preserve payroll history and related records.

---

# Indexes

## Employees

### Unique Email Index

```text
IX_Employee_Email
```

Prevents duplicate employee emails.

---

## Payrolls

### Unique Payroll Index

```text
IX_Payroll_Employee_Month_Year
```

Prevents duplicate payroll generation for the same employee and month.

### Payroll Search Index

```text
IX_Payroll_Month_Year
```

Improves payroll filtering performance.

---

# Stored Procedure

## sp_GetPayrollSummaryByMonth

Returns department-wise payroll summary.

### Parameters

| Parameter | Type |
|---|---|
| @Month | int |
| @Year | int |

### Returns

- Department
- TotalEmployees
- TotalSalary

---

# Seed Users

Default users are added automatically during application startup if the database is empty.

| Role | Email |
|---|---|
| Admin | admin@hrms.com |
| HR | hr@hrms.com |

Passwords are stored using BCrypt hashing.