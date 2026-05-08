# System Architecture

## Architecture Style

This project follows a simple Layered Architecture approach.

The goal is to keep the codebase clean, organized, and easy to maintain while avoiding unnecessary complexity.

Each layer is responsible for a specific part of the application to keep the code easier to manage and maintain.

---

## High-Level Request Flow

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
````

---

## Backend Architecture Overview

### Controllers

Controllers handle incoming HTTP requests and return API responses.

Responsibilities:

* Receive requests
* Call service methods
* Return standardized API responses

Controllers should remain thin and should not contain business logic.

---

### Services

Services contain application logic and business rules.

Responsibilities:

* Handle business operations
* Validate business rules
* Coordinate repositories
* Throw custom exceptions when needed

---

### Repositories

Repositories handle database-related operations using Entity Framework Core.

Responsibilities:

* Query data
* Insert/update/delete records
* Keep EF Core logic separated from services

Repositories should not contain business logic.

---

### ApplicationDbContext

ApplicationDbContext is the main EF Core database context.

Responsibilities:

* Manage database connection
* Configure entities and relationships
* Execute database queries
* Handle migrations

Entity configurations will be added gradually in later phases.

---

## Frontend Architecture Overview

The frontend is built using Angular standalone components.

Main frontend structure:

```text
core/
shared/
features/
```

### core/

Contains application-wide services, guards, and interceptors.

Examples:

* auth.interceptor.ts
* error.interceptor.ts
* auth.guard.ts

---

### shared/

Contains reusable components, utilities, and interfaces.

Examples:

* loading spinner
* shared models
* reusable UI components

---

### features/

Contains feature-specific modules and screens.

Examples:

* employees
* salaries
* payrolls
* dashboard

---

## API Structure

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

## Standard API Response Format

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

## Global Exception Handling

Global exception handling is managed through `ExceptionMiddleware`.

Responsibilities:

* Catch unhandled exceptions
* Return consistent error responses
* Convert exceptions into proper HTTP status codes

Examples:

* 400 Bad Request
* 404 Not Found
* 409 Conflict
* 500 Internal Server Error

---

## Base Entity Design

Common entity fields are managed through shared base classes.

### BaseEntity

Contains common fields such as:

* Id
* CreatedAt
* UpdatedAt

### SoftDeletableEntity

Extends `BaseEntity` and adds soft delete support.

```text
IsDeleted = true
```

Soft delete behavior will be implemented in later phases where necessary.

---

## Security

JWT authentication and role-based authorization
will be implemented in later phases.

Planned roles:

* Admin
* HR

---

## Database Design

The database schema will be finalized gradually
as entities and business requirements are implemented.

Planned core modules include:

* Employee
* Salary
* Payroll
* Authentication

---

## CORS Configuration

CORS is configured to allow Angular frontend requests during development.

Example:

```text
http://localhost:4200
```

---

## Middleware Pipeline

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

