# Human Resource Management System (HRMS)

A full-stack Human Resource Management System built using ASP.NET Core Web API and Angular.

The project is being developed phase-by-phase with a focus on layered architecture, maintainable backend design, and clean development practices.

---

## Tech Stack

### Backend

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- FluentValidation
- JWT Authentication 

### Frontend

- Angular 19

---

## Project Structure

```text
HRMS/
│
├── HRMS.API/
├── hrms-client/
├── Documentation/
└── README.md
```

---

## Backend Architecture

The backend follows a simple layered architecture approach.

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

## Features Planned

- Employee Management
- Salary Management
- Payroll Management
- Role-based Authentication
- Dashboard & Reporting
- PDF Report Generation

---

## Documentation

Project documentation is available inside the `Documentation` folder.

---

## Setup Instructions

### Backend

```bash
cd HRMS.API

dotnet restore

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

### Frontend

```bash
cd hrms-client

ng serve
```

Frontend runs on:

```text
http://localhost:4200
```

---