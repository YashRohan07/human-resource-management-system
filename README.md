# Human Resource Management System (HRMS)

A full-stack Human Resource Management System built using ASP.NET Core Web API and Angular.

The project is being developed phase-by-phase with a focus on layered architecture, maintainable backend design, and clean development practices.

---

# Tech Stack

## Backend

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- FluentValidation
- BCrypt Password Hashing
- JWT Authentication

---

## Frontend

- Angular 19

---

# Current Features

- Employee entity structure
- Salary entity structure
- Payroll entity structure
- Entity relationships
- Soft delete support
- SQL Server integration
- Entity Framework Core migrations
- Database indexes
- Payroll summary stored procedure
- Seed Admin and HR users
- Global exception handling
- CORS configuration
- JWT login endpoint
- Role-based authorization
- Protected route testing
- Admin-only endpoint testing
- Employee CRUD operations
- Employee search and filtering
- Employee pagination support
- Employee sorting support
- Employee soft delete support

---

# Project Structure

```text
HRMS/
│
├── HRMS.API/
├── hrms-client/
├── Documentation/
└── README.md
````

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

Current implemented layers:

* Controllers
* Services
* Repositories
* EF Core DbContext

---

# Database Design

Current core modules:

* Authentication
* Employees
* Salaries
* Payrolls

Main relationships:

```text
Employee → Salary = One-to-Zero-or-One
Employee → Payroll = One-to-Many
```

Employees support soft delete functionality using global query filters.

---

# Security

Current security features:

* BCrypt password hashing
* JWT Authentication
* Role-based Authorization
* Seed Admin and HR users
* Protected API endpoints
* Admin-only endpoint support

---

# Employee Module Features

Current employee module supports:

* Create employee
* Update employee
* Soft delete employee
* Get employee by id
* Employee list pagination
* Search by employee name or email
* Department filtering
* Employment status filtering
* Sorting support

---

# Stored Procedure

```text
sp_GetPayrollSummaryByMonth
```

Used for payroll summary reporting.

---

# Documentation

Project documentation is available inside the `Documentation` folder.

* SYSTEM_ARCHITECTURE.md
* DATABASE_SCHEMA.md
* API_ENDPOINTS.md

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
