# System Architecture

This project follows a layered architecture approach to keep the codebase clean, organized, and maintainable.

---

# High-Level Request Flow

```text
Angular Frontend
        ↓
ASP.NET Core Controllers
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

# Backend Architecture

## Controllers

Controllers handle incoming HTTP requests and return API responses.

### Responsibilities

- Receive requests
- Validate request flow
- Call service methods
- Return API responses

Controllers remain thin and do not contain business logic.

### Controllers

- AuthController
- EmployeesController
- SalariesController
- PayrollsController
- DashboardController

---

## Services

Services contain business logic and application rules.

### Responsibilities

- Handle business operations
- Validate business rules
- Coordinate repositories
- Throw business exceptions when needed

### Services

- AuthService
- EmployeeService
- SalaryService
- PayrollService
- DashboardService

---

## Repositories

Repositories handle database operations using Entity Framework Core.

### Responsibilities

- Query data
- Insert/update/delete records
- Keep EF Core logic separated from services

### Repositories

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

Main modules:

- Authentication
- Employees
- Salaries
- Payrolls
- Dashboard Reporting

---

# Entity Design

## BaseEntity

Contains common fields shared across entities.

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

# Relationships

## Employee → Salary

```text
One-to-One
```

Each employee can have one current salary record.

---

## Employee → Payroll

```text
One-to-Many
```

Each employee can have multiple payroll records.

---

# Payroll Snapshot Design

Payroll records store salary snapshots.

Example:

```text
BasicSalarySnapshot
BonusSnapshot
DeductionSnapshot
```

This keeps historical payroll records unchanged even if salary data changes later.

---

# Payroll Generation Flow

```text
Get active employees
        ↓
Load employee salaries
        ↓
Validate salary effective date
        ↓
Check duplicate payroll
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

# Payroll Calculation

Current payroll formula:

```text
Gross Salary = Basic Salary + Bonus

Tax = Gross Salary × 10%

Net Salary = Gross Salary − Tax − Deduction
```

---

# Dashboard Reporting

Dashboard reporting uses aggregation queries and stored procedure reporting.

### Dashboard Data

- Total employees
- Active employees
- Current month payroll cost
- Department-wise employee statistics
- Department-wise payroll summaries

---

# Stored Procedure

## sp_GetPayrollSummaryByMonth

Used for payroll summary reporting.

### Returns

- Department
- TotalEmployees
- TotalSalary

---

# API Structure

All APIs use versioned routes.

```text
/api/v1/
```

Examples:

```text
/api/v1/auth
/api/v1/employees
/api/v1/salaries
/api/v1/payrolls
/api/v1/dashboard
```

---

# Standard API Response Format

The backend uses a consistent API response structure through `ApiResponse<T>`.

### Success Response

```json
{
  "success": true,
  "message": "Request completed successfully",
  "data": {}
}
```

### Error Response

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

- Employee CRUD operations
- Pagination
- Search and filtering
- Department filtering
- Employment status filtering
- Sorting support
- Soft delete

---

# Salary Module Features

- Create employee salary
- Update salary information
- Salary validation
- Duplicate salary prevention
- Effective date handling

---

# Payroll Module Features

- Monthly payroll generation
- Payroll duplicate prevention
- Payroll pagination
- Payroll filtering
- Employee payroll history
- Payroll reporting
- Automatic tax calculation
- Salary snapshot preservation
- Future payroll prevention

---

# Global Exception Handling

Global exception handling is managed through `ExceptionMiddleware`.

### Responsibilities

- Catch unhandled exceptions
- Return consistent error responses
- Convert exceptions into proper HTTP status codes

### Common Status Codes

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
AuthController receives request
        ↓
AuthService validates credentials
        ↓
PasswordHasher verifies password
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

JWT tokens store:

- User Id
- Email
- Role

---

# Current Security Features

- JWT token generation
- JWT validation
- Role-based authorization
- Protected endpoints
- BCrypt password hashing
- CORS configuration

---

# Database Migrations

Entity Framework Core migrations are used to manage database schema changes.

### Benefits

- Track schema changes
- Keep database structure consistent
- Easier development setup

---

# CORS Configuration

CORS is configured to allow Angular frontend requests during development.

Example:

```text
http://localhost:4200
```

---

# Middleware Pipeline

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