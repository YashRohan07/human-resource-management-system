# API Endpoints

This document contains the current API endpoints of the HRMS project.

Base URL:

```text
/api/v1
```

---

# Authentication APIs

Base Route:

```text
/api/v1/auth
```

| Method | Endpoint      | Description                | Authorization  |
| ------ | ------------- | -------------------------- | -------------- |
| POST   | /login        | Login and return JWT token | Public         |
| GET    | /profile      | Test protected user access | Logged-in User |
| GET    | /admin-only   | Test admin-only access     | Admin Only     |

---

# Login Request Example

## Request

```http
POST /api/v1/auth/login
```

```json
{
  "email": "admin@hrms.com",
  "password": "Admin123!"
}
```

---

## Success Response

```json
{
  "success": true,
  "message": "Login successful",
  "data": {
    "token": "JWT_TOKEN",
    "email": "admin@hrms.com",
    "fullName": "System Admin",
    "role": "Admin",
    "expiresAt": "2026-05-09T16:17:37Z"
  },
  "errors": null
}
```

---

# Employee APIs

Base Route:

```text
/api/v1/employees
```

| Method | Endpoint  | Description                                   | Authorization |
| ------ | --------- | --------------------------------------------- | ------------- |
| GET    | /         | Get employee list with pagination and filters | Admin, HR     |
| GET    | /{id:int} | Get employee details                          | Admin, HR     |
| POST   | /         | Create employee                               | Admin, HR     |
| PUT    | /{id:int} | Update employee                               | Admin, HR     |
| DELETE | /{id:int} | Soft delete employee                          | Admin Only    |

---

# Employee Query Parameters

| Parameter  | Description                                     |
| ---------- | ----------------------------------------------- |
| page       | Page number                                     |
| pageSize   | Number of items per page                        |
| search     | Search by employee name or email                |
| department | Filter by department                            |
| status     | Filter by employment status                     |
| sortBy     | Sort by email, department, status, or createdAt |
| sortOrder  | asc or desc                                     |

---

# Employee Create Request Example

```http
POST /api/v1/employees
```

```json
{
  "fullName": "John Doe",
  "email": "john@hrms.com",
  "phone": "01712345678",
  "department": "HR",
  "position": "HR Manager",
  "accountNumber": "123456789",
  "employmentStatus": "Active"
}
```

---

# Salary APIs

Base Route:

```text
/api/v1/salaries
```

| Method | Endpoint                   | Description                | Authorization |
| ------ | -------------------------- | -------------------------- | ------------- |
| GET    | /employee/{employeeId:int} | Get employee salary        | Admin, HR     |
| POST   | /                          | Create employee salary     | Admin, HR     |
| PUT    | /{id:int}                  | Update employee salary     | Admin, HR     |

---

# Salary Create Request Example

```http
POST /api/v1/salaries
```

```json
{
  "employeeId": 2,
  "basicSalary": 50000,
  "bonus": 5000,
  "deduction": 1000,
  "effectiveFrom": "2026-05-11"
}
```

---

# Salary Update Request Example

```http
PUT /api/v1/salaries/1
```

```json
{
  "basicSalary": 60000,
  "bonus": 7000,
  "deduction": 1500,
  "effectiveFrom": "2026-05-11"
}
```

---

# Salary Validation Rules

| Field         | Validation Rule                  |
| ------------- | -------------------------------- |
| employeeId    | Employee must exist              |
| basicSalary   | Must be greater than 0           |
| bonus         | Cannot be negative               |
| deduction     | Cannot be negative               |
| effectiveFrom | Required                         |

---

# Payroll APIs

Base Route:

```text
/api/v1/payrolls
```

| Method | Endpoint                   | Description                  | Authorization |
| ------ | -------------------------- | ---------------------------- | ------------- |
| POST   | /generate                  | Generate monthly payroll     | Planned       |
| GET    | /                          | Get payroll list             | Planned       |
| GET    | /{id:int}                  | Get payroll details          | Planned       |
| GET    | /employee/{employeeId:int} | Get employee payroll history | Planned       |
| GET    | /summary                   | Get payroll summary          | Planned       |

---

# Dashboard API

Base Route:

```text
/api/v1/dashboard
```

| Method | Endpoint | Description           | Authorization |
| ------ | -------- | --------------------- | ------------- |
| GET    | /        | Get dashboard summary | Planned       |

---

# Authorization Notes

## Public Endpoint

Public endpoints do not require login.

Example:

```text
POST /api/v1/auth/login
```

---

## Protected Endpoint

Protected endpoints require a valid JWT token.

Example:

```text
GET /api/v1/employees
```

---

## Admin Only Endpoint

Admin-only endpoints require:

```text
Role = Admin
```

Example:

```text
DELETE /api/v1/employees/{id}
```

---

# Default Seed Users

| Role  | Email         |
| ----- | ------------- |
| Admin | admin@hrms.com |
| HR    | hr@hrms.com |