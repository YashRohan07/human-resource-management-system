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

Stores system login users.

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

Employee records support soft delete.

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

Stores current salary information for employees.

Each employee can have one current salary record.

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

Payroll values are stored as snapshots so old payroll records remain unchanged even if salary information changes later.

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

Each employee can have one current salary record.

Salary can be assigned after employee creation.

---

## Employee → Payroll

```text
One-to-Many
```

One employee can have multiple payroll records.

Payroll records are kept for payroll history.

---

# Soft Delete

Soft delete is implemented for employees.

Instead of permanently deleting an employee record:

```text
IsDeleted = true
```

Normal employee queries exclude soft deleted records using a global query filter.

This helps preserve payroll history.

---

# Payroll Rules

- Payroll is generated only for active employees
- Employees without salary records are skipped
- Salary effective date must be valid
- Duplicate payroll generation is blocked
- Future month payroll generation is blocked
- Payroll generation uses transactions
- Payroll salary values are stored as snapshots

---

# Dashboard Reporting

Dashboard summary data is generated using employee and payroll aggregation queries.

Dashboard includes:

- Total employees
- Active employees
- Current month payroll cost
- Department-wise employee statistics
- Department-wise payroll summaries

---

# Indexes

## AppUsers

### IX_AppUser_Email

Prevents duplicate login emails.

---

## Employees

### IX_Employee_Email

Prevents duplicate employee emails.

---

## Salaries

### IX_Salaries_EmployeeId

Prevents multiple current salary records for the same employee.

---

## Payrolls

### IX_Payroll_Employee_Month_Year

Ensures only one payroll record exists per employee for a specific month and year.

### IX_Payroll_Month_Year

Improves payroll filtering by month and year.

---

# Stored Procedure

## sp_GetPayrollSummaryByMonth

Used for department-wise payroll summary reporting.

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