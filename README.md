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


<img width="1886" height="1078" alt="11" src="https://github.com/user-attachments/assets/a91bc5e4-3f80-4fd4-a23b-594dc6208934" />

<img width="1892" height="1094" alt="12" src="https://github.com/user-attachments/assets/21243d93-2863-4f30-93b4-691b7ddc03b2" />

<img width="1874" height="1083" alt="13" src="https://github.com/user-attachments/assets/f60b68e1-a0a8-4ef1-a590-3aeb5f94641f" />

<img width="1882" height="1071" alt="14" src="https://github.com/user-attachments/assets/a8280407-443a-4e5e-9c32-98837676a214" />

<img width="1892" height="1071" alt="15" src="https://github.com/user-attachments/assets/77fa5cdf-e83b-4631-b78d-83f62ccb459b" />

<img width="1899" height="1065" alt="16" src="https://github.com/user-attachments/assets/f366aad0-9a25-4e49-b45b-46ba92d96ba9" />

<img width="1898" height="1083" alt="17" src="https://github.com/user-attachments/assets/b2ad723c-356b-4deb-b248-efad39b2fe0c" />

<img width="1894" height="1095" alt="1" src="https://github.com/user-attachments/assets/c11fd8dd-a42d-4f05-bbe4-231f98bf29ad" />

<img width="1888" height="1052" alt="2" src="https://github.com/user-attachments/assets/c181fb49-505d-48a1-99bb-c880e81c498e" />

<img width="1894" height="1090" alt="19" src="https://github.com/user-attachments/assets/9f5ea254-d443-4afe-9968-4ecfe281831b" />

<img width="1886" height="1062" alt="20" src="https://github.com/user-attachments/assets/5cb6146d-e4de-41c0-a373-65014615c01e" />











