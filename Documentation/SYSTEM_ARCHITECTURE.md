# System Architecture

## Architecture Style

This project follows a Layered Architecture approach.

The goal is to keep the codebase clean, organized, and easy to maintain.

Each layer is responsible for a specific part of the application.

---

# High-Level Request Flow

```text
Angular Frontend
        ↓
ASP.NET Core Controller
        ↓
Service Layer
        ↓
Repository Layer
        ↓
ApplicationDbContext
        ↓
SQL Server Database
```

---

# Backend Architecture Overview

## Controllers

Controllers handle incoming HTTP requests and return API responses.

### Responsibilities

- Receive requests
- Call service methods
- Return API responses

Controllers should remain thin and should not contain business logic.

Current implemented controllers:

- AuthController
- EmployeesController
- SalariesController
- PayrollsController
- DashboardController

---

## Services

Services contain application logic and business rules.

### Responsibilities

- Handle business operations
- Validate business rules
- Coordinate repositories
- Throw custom exceptions when needed

Current implemented services:

- AuthService
- EmployeeService
- SalaryService
- PayrollService
- DashboardService

---

## Repositories

Repositories handle database-related operations using Entity Framework Core.

### Responsibilities

- Query data
- Insert/update/delete records
- Keep EF Core logic separated from services

Repositories should not contain business logic.

Current implemented repositories:

- EmployeeRepository
- SalaryRepository
- PayrollRepository

---

## ApplicationDbContext

ApplicationDbContext is the main EF Core database context.

### Responsibilities

- Manage database connection
- Configure entities and relationships
- Configure indexes
- Execute database operations
- Handle migrations
- Configure global query filters

---

# Database Design Overview

Current core modules:

- Authentication
- Employees
- Salaries
- Payrolls
- Dashboard Reporting

The database uses:

- Entity Framework Core
- SQL Server
- Fluent API configurations
- Migrations
- Stored Procedures

---

# Entity Design

## BaseEntity

Contains shared fields used by multiple entities.

### Common Fields

- Id
- CreatedAt
- UpdatedAt

---

## SoftDeletableEntity

Extends `BaseEntity` and adds soft delete support.

### Additional Field

```text
IsDeleted
```

Employees use soft delete to preserve payroll history.

Soft deleted employees are automatically filtered using EF Core global query filters.

---

# Relationship Design

## Employee → Salary

```text
One-to-One
```

Each employee can have zero or one current salary configuration.

Salary records are updated directly without creating salary history rows.

---

## Employee → Payroll

```text
One-to-Many
```

Each employee can have multiple payroll records over time.

Payroll history remains preserved permanently.

---

# Payroll Snapshot Design

Payroll records store salary snapshots.

Example:

```text
BasicSalarySnapshot
BonusSnapshot
DeductionSnapshot
```

This prevents old payroll history from changing if salary values are updated later.

Salary updates affect only future payroll generation.

---

# Payroll Generation Flow

Payroll generation follows this flow:

```text
Get active employees
        ↓
Load employee salaries
        ↓
Validate duplicate payroll
        ↓
Calculate gross salary
        ↓
Calculate tax
        ↓
Calculate net salary
        ↓
Store payroll snapshot
        ↓
Save payroll records using transaction
```

---

# Payroll Calculator

Payroll calculations are separated into a dedicated calculator service.

Current formula:

```text
Gross Salary = Basic Salary + Bonus

Tax = Gross Salary × 10%

Net Salary = Gross Salary − Tax − Deduction
```

---

# Query Models

The project uses query models for stored procedure results.

Example:

```text
PayrollSummaryResult
```

This model is not mapped as a database table.

---

# Stored Procedures

## sp_GetPayrollSummaryByMonth

Used for payroll reporting and summary generation.

Returns:

- Department
- TotalEmployees
- TotalSalary

---

# Dashboard Reporting Architecture

Dashboard reporting uses aggregation queries and stored procedure reporting.

Dashboard summary includes:

- Total employees
- Active employees
- Current month payroll cost
- Department-wise employee statistics
- Department-wise payroll summaries

Dashboard queries are handled directly inside `DashboardService` because the reporting logic is lightweight and aggregation-focused.

---

# Frontend Architecture Overview

The frontend is planned using Angular standalone components.

Main frontend structure:

```text
core/
shared/
features/
```

---

## core/

Contains application-wide services, guards, and interceptors.

Examples:

- auth.interceptor.ts
- error.interceptor.ts
- auth.guard.ts

---

## shared/

Contains reusable components, utilities, and interfaces.

Examples:

- shared models
- reusable UI components
- loading spinner

---

## features/

Contains feature-specific modules and screens.

Examples:

- employees
- salaries
- payrolls
- dashboard

---

# API Structure

All APIs use versioned routes.

```text
/api/v1/
```

Example:

```text
/api/v1/employees
/api/v1/salaries
/api/v1/payrolls
/api/v1/dashboard
```

---

# Standard API Response Format

The backend uses a consistent API response structure through `ApiResponse<T>`.

Example:

```json
{
  "success": true,
  "message": "Request completed successfully",
  "data": {}
}
```

Error example:

```json
{
  "success": false,
  "message": "Validation failed",
  "errors": [
    "Email is required"
  ]
}
```

---

# Employee Module Features

Current employee module supports:

- Employee CRUD operations
- Pagination
- Search by name or email
- Department filtering
- Employment status filtering
- Sorting
- Soft delete

---

# Salary Module Features

Current salary module supports:

- Create employee salary
- Update employee salary
- Get employee salary
- Salary validation rules
- Duplicate salary prevention

---

# Payroll Module Features

Current payroll module supports:

- Monthly payroll generation
- Payroll duplicate prevention
- Payroll transaction handling
- Payroll pagination
- Payroll filtering
- Employee payroll history
- Payroll summary reporting
- Automatic payroll tax calculation

---

# Dashboard Module Features

Current dashboard module supports:

- Total employee count
- Active employee count
- Current month payroll cost
- Department-wise employee statistics
- Department-wise payroll summary reporting

---

# Global Exception Handling

Global exception handling is managed through `ExceptionMiddleware`.

### Responsibilities

- Catch unhandled exceptions
- Return consistent error responses
- Convert exceptions into proper HTTP status codes

Examples:

- 400 Bad Request
- 401 Unauthorized
- 403 Forbidden
- 404 Not Found
- 409 Conflict
- 500 Internal Server Error

---

# Security

The project uses JWT-based authentication and role-based authorization.

## Authentication Flow

```text
User sends email and password
        ↓
AuthController receives login request
        ↓
AuthService verifies user credentials
        ↓
PasswordHasher verifies BCrypt password hash
        ↓
JwtHelper generates JWT token
        ↓
Client receives token
```

---

## Authorization

Protected endpoints use:

```csharp
[Authorize]
```

Admin-only endpoints use:

```csharp
[Authorize(Roles = "Admin")]
```

JWT tokens currently store:

- User Id
- Email
- Role

This allows the API to identify users and control role-based access without server-side sessions.

---

## Current Security Features

- BCrypt password hashing
- JWT token generation
- Role-based authorization
- Protected endpoints
- Admin-only endpoints
- 401 Unauthorized handling
- 403 Forbidden handling

---

# Database Migrations

Entity Framework Core migrations are used to manage database schema changes.

Benefits:

- Keep database schema consistent
- Track schema updates
- Easier setup for development

---

# CORS Configuration

CORS is configured to allow Angular frontend requests during development.

Example:

```text
http://localhost:4200
```

---

# Middleware Pipeline

Current middleware pipeline:

```text
HTTPS Redirection
        ↓
Exception Middleware
        ↓
CORS
        ↓
Authentication
        ↓
Authorization
        ↓
Controllers
```

This setup keeps request handling centralized and easier to maintain.