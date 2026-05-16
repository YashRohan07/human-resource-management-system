# Human Resource Management System (HRMS)

A full-stack Human Resource Management System built using ASP.NET Core Web API and Angular.

The project was developed with a focus on clean layered architecture, maintainable backend design, and scalable payroll management.

---

# Tech Stack

## Backend

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- FluentValidation
- JWT Authentication
- BCrypt Password Hashing

## Frontend

- Angular 19
- Angular Material
- RxJS

---

# Main Features

## Authentication & Security

- JWT Authentication
- Role-based Authorization
- Protected API endpoints
- BCrypt password hashing

---

## Employee Management

- Create employee
- Update employee
- Soft delete employee
- Employee list pagination
- Employee search and filtering
- Department and status filtering
- Sorting support

---

## Salary Management

- Create employee salary
- Update salary information
- Salary validation rules
- Effective date handling
- Duplicate salary prevention

---

## Payroll Management

- Monthly payroll generation
- Payroll duplicate prevention
- Automatic tax calculation
- Payroll history
- Payroll pagination and filtering
- Payroll summary reporting
- Future payroll prevention
- Salary snapshot preservation
- Historical payroll integrity

---

## Dashboard & Reporting

- Total employee count
- Active employee count
- Current month payroll cost
- Department-wise employee statistics
- Department-wise payroll summary

---

# Backend Architecture

The backend follows a layered architecture approach.

```text
Controller
    ↓
Service
    ↓
Repository
    ↓
ApplicationDbContext
    ↓
SQL Server
```

---

# Project Structure

```text
HRMS/
│
├── HRMS.API/
├── hrms-client/
├── Documentation/
└── README.md
```

---

# Database Design

Main modules:

- Authentication
- Employees
- Salaries
- Payrolls
- Dashboard Reporting

Relationships:

```text
Employee → Salary = One-to-One
Employee → Payroll = One-to-Many
```

Soft delete is implemented using:

```text
IsDeleted = true
```

---

# API Structure

All APIs use versioned routes.

```text
/api/v1/
```

Example:

```text
/api/v1/auth
/api/v1/employees
/api/v1/salaries
/api/v1/payrolls
/api/v1/dashboard
```

---

# Payroll Business Rules

- Payroll generates only for active employees
- Salary effective date must be valid
- Duplicate payroll generation is blocked
- Future payroll generation is prevented
- Payroll salary values are stored as snapshots

---

# Security

- JWT token generation and validation
- Role-based endpoint protection
- Protected routes
- BCrypt password hashing
- CORS configuration

---

# Stored Procedure

```text
sp_GetPayrollSummaryByMonth
```

Used for payroll summary reporting.

---

# Documentation

Detailed documentation is available inside the `Documentation` folder.

- SYSTEM_ARCHITECTURE.md
- DATABASE_SCHEMA.md
- API_ENDPOINTS.md

---

# Setup Instructions

## Backend

```bash
cd HRMS.API

dotnet restore

dotnet ef database update

dotnet run
```

Backend runs on:

```text
http://localhost:5243
```

Swagger:

```text
http://localhost:5243/swagger
```

---

## Frontend

```bash
cd hrms-client

npm install

ng serve
```

Frontend runs on:

```text
http://localhost:4200
```

---

# Default Login Credentials

## Admin

```text
Email: admin@hrms.com
Password: Admin123!
```

## HR

```text
Email: hr@hrms.com
Password: Hr123!
```