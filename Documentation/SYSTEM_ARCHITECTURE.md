# System Architecture

## Architecture Style

This project follows a Layered Architecture approach.

The goal is to keep the codebase clean, organized, and easy to maintain without adding unnecessary complexity.

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

---

## Services

Services contain application logic and business rules.

### Responsibilities

- Handle business operations
- Validate business rules
- Coordinate repositories
- Throw custom exceptions when needed

---

## Repositories

Repositories handle database-related operations using Entity Framework Core.

### Responsibilities

- Query data
- Insert/update/delete records
- Keep EF Core logic separated from services

Repositories should not contain business logic.

---

## ApplicationDbContext

ApplicationDbContext is the main EF Core database context.

### Responsibilities

- Manage database connection
- Configure entities and relationships
- Configure indexes
- Execute database operations
- Handle migrations

---

# Database Design Overview

Current core modules:

- Authentication
- Employees
- Salaries
- Payrolls

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

---

# Relationship Design

## Employee → Salary

```text
One-to-One
```

Each employee has one current salary configuration.

---

## Employee → Payroll

```text
One-to-Many
```

Each employee can have multiple payroll records over time.

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

# Frontend Architecture Overview

The frontend is built using Angular standalone components.

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

# Global Exception Handling

Global exception handling is managed through `ExceptionMiddleware`.

### Responsibilities

- Catch unhandled exceptions
- Return consistent error responses
- Convert exceptions into proper HTTP status codes

Examples:

- 400 Bad Request
- 404 Not Found
- 409 Conflict
- 500 Internal Server Error

---

# Security

Authentication and authorization will be expanded in later phases.

Current security features:

- BCrypt password hashing
- Role field support
- Seed admin users

Planned:

- JWT Authentication
- Role-based Authorization

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
Controllers
```

This setup keeps request handling centralized and easier to maintain.