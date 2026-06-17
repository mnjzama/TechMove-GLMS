# TechMove Global Logistics Management System

## 🌍 Overview

TechMove is a full-stack, containerised Global Logistics Management System developed as part of the Programming 7311 module.

The system implements a modern **API-first architecture** designed to simulate an enterprise-grade logistics platform.

It is composed of the following core components:

- ASP.NET Core Web API (Backend services)
- ASP.NET Core MVC Client (Frontend application)
- SQL Server Database (Docker containerised environment)
- Supabase Storage (Secure file handling and document storage)

The system demonstrates industry-aligned software engineering practices, including:

- Clear separation of concerns (API, Client, Data layers)
- Automated testing (Unit, API, and UI testing)
- Continuous Integration and Deployment (GitHub Actions CI/CD pipeline)
- Full containerisation using Docker and Docker Compose for consistent deployment

---

## 📖 Project Background

TechMove Logistics is a global shipping coordinator responsible for managing international freight contracts, service requests, and logistics operations. The company previously relied on a legacy environment consisting of disconnected spreadsheets, emails, and manual communication processes.

This approach resulted in several operational challenges, including:

* Fragmented and inconsistent data management
* Lost or misplaced invoices and documentation
* Compliance risks caused by expired contracts
* Manual workflow bottlenecks and reduced efficiency
* Limited scalability for future business growth

To address these challenges, TechMove commissioned the development of the **Global Logistics Management System (GLMS)**, an enterprise-grade web platform designed to centralise operations, improve data integrity, and support future expansion.

The solution was developed using a modern API-first architecture and demonstrates enterprise software engineering principles including layered architecture, design patterns, automated testing, CI/CD pipelines, and containerised deployment.

### 🎯 Project Objectives

The primary objective of GLMS is to provide a scalable and maintainable logistics management platform capable of:

* Managing clients, contracts, and service requests through a centralised system
* Enforcing business rules and contract validation
* Supporting international operations through automated currency conversion
* Improving operational efficiency through workflow automation
* Providing a foundation for future service-oriented and distributed architectures
* Supporting deployment and scalability through Docker containerisation and orchestration

### 🚀 Key Business Features

#### Contract Management Hub

* Centralised storage of client contracts and agreements
* Service Level Agreement (SLA) management
* Contract lifecycle tracking (Draft, Active, Expired, On Hold)

#### Service Request Processing

* Creation of service requests linked to contracts
* Validation to ensure requests are only raised against valid contracts
* Status tracking throughout the service request lifecycle

#### Financial Integration

* Automated international currency conversion
* Integration with external exchange rate services
* Dynamic cost calculations for logistics services

#### Modern Enterprise Architecture

* Separation of frontend and backend concerns
* API-driven communication
* Containerised deployment using Docker
* CI/CD automation through GitHub Actions
* Foundation for future scalability and distributed services

---

## ⚙️ Technology Stack

- ASP.NET Core Web API (.NET 10)
- ASP.NET Core MVC (.NET 10)
- Entity Framework Core
- SQL Server (Docker)
- Supabase Storage (File uploads)
- xUnit (Unit Testing)
- Newman (API Testing)
- Playwright (UI Testing)
- GitHub Actions (CI/CD)
- Docker & Docker Compose

---

## 📦 Key NuGet Packages

### API Project (TechMoveAPI)

- Entity Framework Core (SQL Server)
- JWT Bearer Authentication
- Swagger / OpenAPI
- Newtonsoft.Json
- Firebase Authentication

### Client Project (TechMoveClient)

- Entity Framework Core
- Supabase SDK
- Firebase Authentication

### Test Project (ProjectTests)

- xUnit
- Moq
- FluentAssertions
- Entity Framework Core In-Memory Provider
- Coverlet Code Coverage

---

## 🚀 Installation & Setup

### 1. Clone the Repository

```bash
git clone https://github.com/EMKNDW/prog7311-g2-2026-poe-ST10450036.git
```

### 2. Navigate into the Project Folder

```bash
cd prog7311-g2-2026-poe-ST10450036
```

### 3. Restore Dependencies

```bash
dotnet restore TechMove.sln
```

---

## 🔐 Environment Configuration

Before running the application, create a `.env` file in the root directory of the project.

You may use the provided `.env.example` file as a template.

```bash
cp .env.example .env
```

### Required Variables

```env
SA_PASSWORD=
TechMoveJwtKey=
FirebaseTechMove=
SUPABASE_URL=
SUPABASE_KEY=
SUPABASE_BUCKET=
```

### Variable Descriptions

| Variable | Description |
|-----------|-------------|
| SA_PASSWORD | SQL Server administrator password used by the Docker database container |
| TechMoveJwtKey | Secret key used for JWT authentication and token generation |
| FirebaseTechMove | Firebase API key used for authentication services |
| SUPABASE_URL | URL of your Supabase project |
| SUPABASE_KEY | Supabase API key used to access storage services |
| SUPABASE_BUCKET | Name of the Supabase storage bucket used for contract documents |

### External Service Requirements

To fully utilise all application functionality, the following services must be configured:

- A Supabase project with a storage bucket for contract documents
- A Firebase project for authentication services

> External service credentials are not included in the repository for security reasons and must be supplied separately.

---

## 🗄️ Database Setup (Docker SQL Server)

The database runs inside a Docker container as part of the system architecture.

### Connection String Example

```json
"ConnectionStrings": {
  "TechMoveDB": "Server=localhost,1433;Database=TechMoveDB;User Id=docker123;Password=docker123;TrustServerCertificate=True;"
}
```

---

## 🔄 EF Core Migrations

### Create Migration

```bash
dotnet ef migrations add InitialCreate
```

### Apply Migration

```bash
dotnet ef database update
```

---

## 🧱 System Architecture

The system follows a clean layered architecture:

1. Presentation Layer (MVC Client)
2. API Layer (Controllers + Endpoints)
3. Service Layer (Business Logic)
4. Data Layer (Entity Framework Core)
5. Testing Layer (Unit + Integration Tests)
6. Infrastructure Layer (Docker + CI/CD)

---

### 📁 File Storage Architecture (Hybrid Approach)

The system uses a hybrid file storage model to handle contract document uploads in a containerised environment.

#### API Layer (Local Storage)
- Stores uploaded contract files in the container directory (`wwwroot/uploads/contracts`)
- Handles file validation and deletion through the `FileService`
- Ensures compatibility with legacy file handling logic

#### Client Layer (Supabase Storage)
- Uploads and manages contract documents in Supabase Storage
- Returns publicly accessible file URLs to the API
- Handles external file persistence and accessibility

#### Reason for Hybrid Approach

This design decision was made due to Docker container limitations experienced during development, specifically file permission issues when writing to local directories.

After consultation with my module lecturer, it was confirmed that:
- Containerised file systems require explicit configuration for persistence
- Docker volumes can be used for local persistence
- External storage solutions (such as Supabase) provide a more scalable alternative

As a result, the hybrid approach was adopted to balance:
- Local development stability (API file handling)
- Cloud-based scalability (Supabase storage)
- Containerisation constraints (Docker environment limitations)

This demonstrates an adaptive architectural approach aligned with real-world distributed system design.

---

## 🧩 Design Patterns

### Factory Pattern

Used for contract creation based on service level.

### Strategy Pattern

Used for currency conversion logic in Service Requests.

### Observer Pattern

Used for notifications when Service Request status changes.

---

## 🌐 API Features

* RESTful API design
* Proper HTTP verbs (GET, POST, PATCH, DELETE)
* Correct status codes:
  * 200 OK
  * 201 Created
  * 400 Bad Request
  * 404 Not Found
* Swagger documentation enabled

---

## 💻 Frontend (Client) Features

* Decoupled from database (API-only communication)
* Service layer (HttpClient-based)
* Authentication (Cookies)
* Supabase file upload integration
* Clean separation of concerns

---

## 📂 Core Features

### Client Management

* Create, edit, delete clients
* Prevent deletion if contracts exist

### Contract Management

* Create contracts linked to clients
* Upload PDF agreements
* Supabase storage integration
* Delete contracts (Draft only)
* Auto file cleanup on delete

### Service Requests

* Linked to contracts
* Currency support (USD / EUR)
* Automatic cost calculation
* Status workflow:
  * Pending - In Progress - Completed

---

## 🧪 Automated Testing

### Unit Tests (xUnit)

* Business logic validation
* Currency conversion
* Contract rules
* File validation

### API Tests (Newman)

* Endpoint validation
* CRUD operations
* Authentication flows

### UI Tests (Playwright)

* End-to-end user workflows
* Client - Contract - Service Request flows
* Full system validation

---

## 🧪 Running Tests

```bash
dotnet test TechMove.sln
```

Expected result:

```
Test summary: total: X, passed: X, failed: 0
```

---

## 🐳 Docker Configuration

### Build Containers

```bash
docker-compose build
```

### Run System

```bash
docker-compose up
```

### Services

* API - glms-backend-api
* Client - glms-frontend-web
* Database - sql-server-db

---

## 🌐 Container Orchestration

Docker Compose ensures:

* API + Client + DB run together
* Service-to-service networking
* Environment variables injected automatically
* Consistent deployment across environments

---

## 🔁 CI/CD Pipeline (GitHub Actions)

Pipeline includes:

1. Restore dependencies
2. Build solution
3. Run unit tests
4. Run API tests (Newman)
5. Run UI tests (Playwright)
6. Docker build validation
7. Stack deployment (run-stack job)

---

## ▶️ Running the Application

```bash
docker-compose up --build
```

Then access:

* Client: [http://localhost:8082](http://localhost:8082)
* API: [http://localhost:8081](http://localhost:8081)
* Swagger: [http://localhost:8081/swagger](http://localhost:8081/swagger)

---

## 📁 Folder Structure

```
TechMoveAPI/
TechMoveClient/
db/
ProjectTests/
docker-compose.yml
compose.ci.yml
package-lock.json
package.json
.github/workflows/
.env.example
```

---

## ⚠️ Important Notes

* Ensure Docker Desktop is running
* API must be running before Client
* Supabase bucket must exist (`techmove-contracts`)
* Migrations must be applied before startup
* CI pipeline must pass for deployment readiness

---

## 🎥 System Demonstration

Full system walkthrough available:

[https://youtu.be/gG51XCBA4og](https://youtu.be/gG51XCBA4og)

---

## 👤 Author

* Developed by: Mandlenkosi Njabulo Zama (ST10450036)
* Module: Programming 7311 / PROG7311
* Project: TechMove Global Logistics Management System

---
