[README.md](https://github.com/user-attachments/files/26092895/README.md)
# CRM System

A full-stack enterprise CRM (Customer Relationship Management) web application built with **ASP.NET Core 8**, **Entity Framework Core**, **MySQL**, and **React** (frontend added in a later phase). The project follows **Clean Architecture** principles to ensure separation of concerns, testability, and scalability.

---

## Project Status

| Phase | Name | Status |
|-------|------|--------|
| Phase 1 | Core CRM Foundation | ✅ Complete |
| Phase 2 | Sales Pipeline | 🟡 In Progress |
| Phase 3 | Task & Activity System | ⬜ Not Started |
| Phase 4 | Reporting & Analytics | ⬜ Not Started |
| Phase 5 | Advanced Features | ⬜ Not Started |

---

## Phase 1 — What Was Built

### Features
- ✅ User authentication — register and login with JWT
- ✅ Companies management — full CRUD
- ✅ Contacts management — full CRUD with search
- ✅ Notes — create and delete notes per contact
- ✅ Swagger UI — fully documented and testable API
- ✅ 19 unit tests passing — all services tested in isolation

### API Endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/api/auth/register` | ❌ | Register new user |
| POST | `/api/auth/login` | ❌ | Login and receive JWT |
| GET | `/api/contacts` | ✅ | Get all contacts for user |
| POST | `/api/contacts` | ✅ | Create a contact |
| GET | `/api/contacts/{id}` | ✅ | Get contact by ID |
| PUT | `/api/contacts/{id}` | ✅ | Update a contact |
| DELETE | `/api/contacts/{id}` | ✅ | Delete a contact |
| GET | `/api/contacts/search` | ✅ | Search contacts by keyword |
| GET | `/api/companies` | ✅ | Get all companies |
| POST | `/api/companies` | ✅ | Create a company |
| GET | `/api/companies/{id}` | ✅ | Get company by ID |
| PUT | `/api/companies/{id}` | ✅ | Update a company |
| DELETE | `/api/companies/{id}` | ✅ | Delete a company |
| GET | `/api/notes/contact/{id}` | ✅ | Get notes for a contact |
| POST | `/api/notes` | ✅ | Create a note |
| DELETE | `/api/notes/{id}` | ✅ | Delete a note |

### Unit Test Results

```
✅ AuthServiceTests       — 5 tests passing
✅ ContactServiceTests    — 8 tests passing
✅ CompanyServiceTests    — 3 tests passing
✅ NoteServiceTests       — 3 tests passing
─────────────────────────────────────────
   Total                  — 19 tests passing
```

---

## Architecture

This project follows **Clean Architecture** with a strict inward dependency rule.

```
CRM/
 ├─ CRM.sln
 └─ src/
     ├─ CRM.Domain/            # Entities, interfaces, business rules
     ├─ CRM.Application/       # DTOs, service interfaces, use cases
     ├─ CRM.Infrastructure/    # EF Core, MySQL, repositories, JWT
     ├─ CRM.API/               # ASP.NET Core controllers, DI wiring
     └─ CRM.Tests/             # xUnit unit tests
```

### Dependency Flow

```
React Frontend
     ↓  HTTP/JSON
CRM.API  (ASP.NET Core Web API)
     ↓  Service interfaces
CRM.Application  (Use cases, DTOs)
     ↓  Repository interfaces
CRM.Infrastructure  (EF Core, MySQL)
     ↓  Queries
MySQL Database
```

### Project Reference Rules

| Project | References |
|---------|------------|
| `CRM.Domain` | None |
| `CRM.Application` | `CRM.Domain` |
| `CRM.Infrastructure` | `CRM.Application` |
| `CRM.API` | `CRM.Application`, `CRM.Infrastructure` |
| `CRM.Tests` | `CRM.Application`, `CRM.Infrastructure` |

---

## Tech Stack

| Technology | Version | Purpose |
|------------|---------|---------|
| .NET | 8.0 | Backend runtime |
| ASP.NET Core Web API | 8.0 | REST API layer |
| Entity Framework Core | 8.0.0 | ORM |
| Pomelo EF Core MySQL | 8.0.0 | MySQL provider |
| MySQL | 8.x | Primary database |
| BCrypt.Net | Latest | Password hashing |
| JWT Bearer | 8.0.0 | Authentication |
| Swashbuckle (Swagger) | 6.8.1 | API documentation |
| xUnit | Latest | Unit testing |
| Moq | 4.20.70 | Mocking framework |
| FluentAssertions | 6.12.0 | Test assertions |
| React | TBD | Frontend (Phase 2+) |

---

## Prerequisites

- [Visual Studio 2022](https://visualstudio.microsoft.com/) (v17.x+)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL Server 8.x](https://dev.mysql.com/downloads/mysql/)
- [MySQL Workbench](https://dev.mysql.com/downloads/workbench/) *(optional)*
- [Git](https://git-scm.com/)

---

## Setup & Installation

### 1. Clone the repository

```bash
git clone https://github.com/your-username/CRM.git
cd CRM
```

### 2. Open in Visual Studio 2022

Open `CRM.sln` in Visual Studio 2022.

### 3. Configure secrets

Create `src/CRM.API/appsettings.Development.json` (this file is in `.gitignore` and will never be committed):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=127.0.0.1;Port=3306;Database=CRMDb;User=root;Password=yourpassword;"
  },
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "CRM.API",
    "Audience": "CRM.Client",
    "ExpiryMinutes": "60"
  }
}
```

### 4. Apply database migrations

Open **Package Manager Console** in Visual Studio and run:

```powershell
Update-Database -Project CRM.Infrastructure -StartupProject CRM.API
```

### 5. Run the API

- Set `CRM.API` as the startup project
- Press **F5**
- Swagger UI opens at `https://localhost:{port}/swagger`

---

## Running Tests

Open **Test Explorer** in Visual Studio (Test → Test Explorer) and click **Run All Tests**.

Or via CLI:

```bash
dotnet test src/CRM.Tests/CRM.Tests.csproj
```

Expected result:
```
Passed: 19  Failed: 0  Skipped: 0
```

---

## Git Workflow

This project uses branch protection on `main`. All changes must go through a Pull Request.

```bash
# Start a new feature
git checkout main
git pull origin main
git checkout -b feature/your-feature

# Save your work
git add .
git commit -m "Add your feature"
git push origin feature/your-feature

# Open PR on GitHub → merge → clean up
git checkout main
git pull origin main
git branch -d feature/your-feature
```

---

## Phase 1 — Git Commit History

```
✅ Initial commit: Phase 1 solution scaffold
✅ Add domain entities: User, Company, Contact, Note with BaseEntity
✅ Add ITokenService interface to Domain layer
✅ Add Application layer: DTOs and service implementations
✅ Add Infrastructure layer: DbContext, repositories and TokenService
✅ Add API layer: controllers, Program.cs and JWT configuration
✅ Add unit tests for Auth, Contact, Company and Note services
```

---

## Roadmap

### Phase 2 — Sales Pipeline *(Next)*
- Leads management
- Sales pipeline with deal stages
- Activities tracking
- Tables: Leads, Deals, DealStages, Activities

### Phase 3 — Task & Activity System
- Tasks and follow-ups
- Meeting and call logs
- Reminders

### Phase 4 — Reporting & Analytics
- Sales dashboards
- Conversion rate metrics
- Pipeline analytics

### Phase 5 — Advanced Features
- Role-based access control (RBAC)
- File uploads
- Email integration
- Docker support
- Automation workflows

---

## Docker *(Planned — Phase 5)*

Docker support will be added in Phase 5 when the API is stable and ready to ship.

---

## License

MIT License — see [LICENSE](LICENSE) for details.
