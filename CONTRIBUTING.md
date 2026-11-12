
# Contributing to SumaqAgro Backend

Thank you for contributing to the SumaqAgro RESTful API, built by the **Dymbia** team. This document sets the architecture rules, coding standards and development workflow that keep the codebase consistent while several team members work on different bounded contexts.

---

## Table of Contents

1. [Architecture & Design Principles](#architecture--design-principles)
2. [Git Workflow & Branching Strategy](#git-workflow--branching-strategy)
3. [Conventional Commits](#conventional-commits)
4. [Semantic Versioning (SemVer)](#semantic-versioning-semver)
5. [C# Guidelines](#c-guidelines)
6. [ASP.NET Web API Standards](#aspnet-web-api-standards)
7. [REST API Standards](#rest-api-standards)
8. [Configuration & Environments](#configuration--environments)
9. [Quality Assurance & Development Workflow](#quality-assurance--development-workflow)
10. [Pull Request (PR) Process](#pull-request-pr-process)

---

## Architecture & Design Principles

### Domain-Driven Design (DDD)

The code is organized by **bounded context** under the `Dymbia.SumaqAgro.Platform` namespace. Namespace names match the names used by the Web Application whenever possible, so both repositories speak the same language.

| Namespace | Bounded Context | Responsibility |
|---|---|---|
| `Shared` | Shared Kernel | Base classes, `Result` and `ApplicationError`, error handling, naming conventions and configuration used by every context. |
| `Farming` | Farming (Field Management) | Plots with their GPS polygon, crop campaigns and the cost ledger with the breakeven price. |
| `Crop` | Crop Health | Satellite monitoring (NDVI/NDWI), climate forecasts, agroclimatic alerts, pest reports, inspections and prescriptions. |
| `Profiles` | Profiles | Profiles, cooperatives and their members, and the assignment of agronomists to plots. |
| `HarvestCertification` | Harvest Certification | Harvest batches, quality grading and certificates with public QR traceability. |
| `Iam`, `Subscriptions` | IAM, Subscriptions and Payments | Users and authentication; plans, payments and the plot quota. |

A context that is not in the code yet is designed in the class diagrams of the project report.

Each bounded context follows the same four layers:

```text
Dymbia.SumaqAgro.Platform.<BoundedContext>/
├── Domain/
│   └── Model/
│       ├── Aggregates/       # Aggregate roots extending AbstractDomainAggregateRoot
│       ├── Entities/         # Entities that live inside an aggregate
│       ├── ValueObjects/     # Value objects and enums
│       ├── Commands/         # Commands that state an intention
│       ├── Queries/          # Queries that request information
│       └── Events/           # Domain events
├── Application/
│   ├── CommandServices/      # Interfaces for operations that change data
│   ├── QueryServices/        # Interfaces for operations that read data
│   ├── Internal/             # Service implementations and event handlers
│   └── OutboundServices/     # Ports for external dependencies
├── Infrastructure/
│   ├── Persistence/
│   │   ├── Entities/         # Entity Framework persistence entities
│   │   ├── Assemblers/       # Aggregate and persistence entity mapping
│   │   ├── Repositories/     # Repository implementations
│   │   └── Context/          # Entity Framework DbContext
│   └── ExternalServices/     # External API adapters
└── Interfaces/
    ├── Rest/                 # API controllers, resources and assemblers
    └── Acl/                  # Facades for communication between contexts
```

Dependency rules between layers:

| Code in | May use |
|---|---|
| `Interfaces` | `Application`, `Domain` and `Shared` |
| `Application` | `Domain`, repository abstractions and outbound service ports |
| `Domain` | Only `Shared/Domain` |
| `Infrastructure` | `Domain`, `Application` interfaces and `Shared` |

Bounded contexts do not directly use each other's services, repositories or aggregates.

They communicate through:

- **Domain events**: A context publishes an event and another context reacts to it.
- **ACL facades**: A context exposes a small interface that returns simple values, for example `FarmingContextFacade`.

An aggregate references another aggregate only by its identifier (`long plotId`), never by the complete object.

### Object-Oriented Programming (OOP) & Clean Code

- **SOLID**: Each class has one responsibility. Controllers handle HTTP requests, services coordinate use cases, repositories manage persistence and assemblers convert data.
- **CQRS**: Command services modify application state and query services retrieve information.
- **Rich Domain Model**: Business rules belong to aggregates and value objects, not controllers.
- **Persistence separated from domain**: Domain aggregates remain independent of Entity Framework.
- **Encapsulation**: Aggregate properties use private setters when appropriate. State changes occur through explicit domain methods.
- **Value Objects**: Values without identity validate themselves during construction.
- **Business Rules**: Rules requiring database access belong to application services. Rules based only on aggregate state belong to the aggregate.

---

## Git Workflow & Branching Strategy

We follow the **Git Flow** branching model. Visual Studio, VS Code or Git commands can be used to manage branches.

```text
main ──────────────────────────────────────────●────── (Releases, tagged vX.Y.Z)
         \                                    /
develop ──●─────────●───────────────●────────●──────── (Integration branch)
           \       /                 \      /
feature/    ●─────●                   ●────●           (Feature branches)
```

- `main`: Released code, tagged with SemVer (`v1.0.0`).
- `develop`: Integration branch where completed features are merged.
- `feature/<context>-<short-description>`: New features, refactors, fixes and documentation.
  - Examples: `feature/farming-crop-campaigns`, `feature/docs-dotnet-framework`.
  - Always use the `feature/` prefix.
- `release/v<MAJOR.MINOR.PATCH>`: Release preparation.
- `hotfix/v<MAJOR.MINOR.PATCH>`: Urgent fixes for released versions.

Before starting a branch, update `develop`:

```bash
git checkout develop
git pull origin develop
git checkout -b feature/<context>-<short-description>
```

Every member commits their own work. Do not modify another member's bounded context without prior agreement.

Changes in `Shared` affect every bounded context and must be communicated before merging.

---

## Conventional Commits

Commit messages follow the [Conventional Commits v1.0.0](https://www.conventionalcommits.org/) specification.

```text
<type>(<scope>): <short summary in imperative mood>

[optional body explaining why the change was made]

[optional footer, such as BREAKING CHANGE]
```

| Type | Description |
|---|---|
| `feat` | A new feature, aggregate, service or endpoint |
| `fix` | A bug fix |
| `docs` | Documentation changes |
| `refactor` | Code changes without adding features or fixing bugs |
| `perf` | Performance improvements |
| `test` | Adding or updating tests |
| `build` | Build configuration or dependency changes |
| `chore` | Maintenance tasks |
| `revert` | Reverts a previous commit |

The scope identifies the aggregate or area being modified:

`field-plot`, `crop-campaign`, `campaign-ledger`, `crop-health`, `farming`, `profiles`, `harvest-certification`, `iam`, `subscriptions`, `shared`, `configuration`, `deps`.

Inside a bounded context, commits should contain small, logically related changes.

Examples:

- `feat(field-plot): add FieldPlot aggregate`
- `feat(crop-campaign): implement command and query services`
- `refactor(farming): standardize namespace structure`
- `feat(shared): add standardized error handling`
- `docs: update development commands for .NET Framework`

---

## Semantic Versioning (SemVer)

Versions follow [SemVer 2.0.0](https://semver.org/): `MAJOR.MINOR.PATCH`.

Every release is documented in [CHANGELOG.md](CHANGELOG.md).

- **MAJOR**: Incompatible changes to the REST API contract.
- **MINOR**: New backward-compatible features.
- **PATCH**: Backward-compatible bug fixes.

---

## C# Guidelines

- **Programming Language**: C#.
- **Framework**: .NET Framework with ASP.NET Web API.
- **Development Environment**: Visual Studio.
- **Dependency Management**: NuGet.
- **Build System**: MSBuild.

### Naming Conventions

- `PascalCase`: Classes, methods, properties, interfaces and enums.
- `camelCase`: Local variables and parameters.
- `_camelCase`: Private instance fields.
- `PascalCase`: Public constants and enum members.
- Namespaces use PascalCase, such as `Farming` and `Crop`.

### Class Names by Role

| Role | Name | Example |
|---|---|---|
| Command / Query | `<Verb><Noun>Command`, `Get<Noun>By<Criteria>Query` | `RegisterFieldPlotCommand`, `GetFieldPlotByIdQuery` |
| Services | `I<Aggregate>CommandService`, `I<Aggregate>QueryService` | `IFieldPlotCommandService` |
| Implementations | `<Aggregate>CommandServiceImpl` | `FieldPlotCommandServiceImpl` |
| Repository | `I<Aggregate>Repository` | `IFieldPlotRepository` |
| Resources | `<Verb><Noun>Resource`, `<Noun>Resource` | `RegisterFieldPlotResource`, `FieldPlotResource` |
| Assemblers | `<Noun>ResourceFromEntityAssembler` | `FieldPlotResourceFromEntityAssembler` |
| Controllers | `<Aggregates>Controller` | `FieldPlotsController` |
| Events | `<Aggregate><PastParticiple>Event` | `CropCampaignStartedEvent` |

### Classes and Validation

- Use C# classes for commands, queries, resources and domain entities.
- Validate required values through constructors or validation methods.
- Use `ArgumentException` for invalid argument values.
- Use `ArgumentNullException` for required null arguments.
- Use properties with private setters to protect aggregate state.
- Avoid exposing mutable collections directly.

### Value Objects

- Implement value objects as immutable C# classes or structs when appropriate.
- Validate values during construction.
- Use enums for statuses and predefined categories.
- Configure Entity Framework mappings separately from domain logic.

### Money and Measures

- Use `decimal` for monetary values and calculations requiring decimal precision.
- Avoid `double` for financial calculations.
- Use appropriate numeric types for geographic coordinates and scientific measurements.

### Comments

- Write XML documentation comments in English for public classes and methods when useful.
- Use `/// <summary>` to describe public APIs.
- Use `/// <param>` and `/// <returns>` when applicable.
- Use short `//` comments only for logic that is not obvious.

---

## ASP.NET Web API Standards

### Dependency Injection

- Use constructor injection for application services and repositories.
- Depend on interfaces rather than concrete implementations.
- Configure dependencies through a compatible dependency injection container.
- Avoid creating service dependencies directly inside controllers.

### Transactions

- Use Entity Framework transactions when multiple database operations must succeed or fail together.
- Use `DbContext.Database.BeginTransaction()` when an explicit transaction is required.
- Call `SaveChanges()` or `SaveChangesAsync()` to persist changes.
- Ensure transactions are committed or rolled back correctly.

### Error Handling

- Application services return `Result<T, ApplicationError>` when business operations may fail.
- Use standardized application errors for not-found resources, conflicts and validation failures.
- Use exceptions for unexpected failures and invalid programming states.
- Do not expose internal exception details to API consumers.

### Controllers

Controllers should:

- Receive HTTP requests.
- Validate incoming request models.
- Convert resources into commands or queries.
- Invoke application services.
- Return appropriate HTTP responses.

Controllers must not contain business rules or direct database access.

### Domain Events

- Aggregates register domain events when meaningful business actions occur.
- Events are published through `DomainEventPublisher`.
- Event handlers execute the corresponding application behavior.
- Coordinate event publication with persistence to avoid inconsistent state.

### External Systems

External services such as AgroMonitoring must be accessed through application ports and infrastructure adapters.

Never call external APIs directly from domain entities or controllers.

Never expose API keys in source code, logs or HTTP responses.

### Persistence

- Use Entity Framework for database access.
- Use MySQL as the relational database.
- Define persistence entities and mappings separately from domain aggregates.
- Use `DbContext` for persistence operations.
- Maintain audit fields such as `Id`, `CreatedAt` and `UpdatedAt`.
- Use repository implementations to isolate persistence logic.

---

## REST API Standards

### Endpoints

Use versioned, resource-oriented paths:

`/api/v1/<plural-resource>`

Examples:

```http
POST /api/v1/field-plots
GET /api/v1/field-plots/{id}
```

### HTTP Status Codes

| Code | When |
|---|---|
| `200 OK` | Successful read or update |
| `201 Created` | A resource was created |
| `204 No Content` | A resource was deleted |
| `400 Bad Request` | Invalid request data |
| `404 Not Found` | Requested resource does not exist |
| `409 Conflict` | Business rule violation |

### JSON

- Use `camelCase` for JSON property names.
- Use ISO 8601 for date and time values.
- Keep property names consistent with the Vue.js Web Application.
- Return consistent response structures across bounded contexts.

### Errors

Use standardized error responses containing:

- `type`
- `title`
- `status`
- `detail`
- `code`

Follow RFC 9457 problem details where supported by the application's response implementation.

### Localization

- Support English and Spanish error messages.
- Use the `Accept-Language` header to determine the response language.
- Use English as the default language.
- Store translated messages in .NET resource files when appropriate.

### Documentation

- Document RESTful endpoints using Swagger/OpenAPI with a library compatible with ASP.NET Web API on .NET Framework.
- Include request parameters, response models and HTTP status codes.
- Verify the Swagger UI URL using the application's actual IIS Express configuration.

---

## Configuration & Environments

Use .NET Framework configuration files to manage application settings.

- `Web.config`: Common application configuration.
- `Web.Debug.config`: Development configuration transformations.
- `Web.Release.config`: Release configuration transformations.

Database connection strings and external API credentials must be configured securely.

Secrets must never be committed to the repository.

### Environment Variables

| Variable | Used for |
|---|---|
| `DATABASE_USER` | MySQL username |
| `DATABASE_PASSWORD` | MySQL password |
| `DATABASE_URL` | MySQL server address |
| `DATABASE_PORT` | MySQL port |
| `DATABASE_NAME` | MySQL database name |
| `AGROMONITORING_API_KEY` | AgroMonitoring API credentials |

The application must explicitly read the required environment variables through its configuration implementation.

In Visual Studio, configure the required settings for the local development environment.

Do not store production passwords or API keys directly in `Web.config`.

---

## Quality Assurance & Development Workflow

### Prerequisites

- Visual Studio with .NET Framework development tools.
- The .NET Framework Developer Pack matching the project's target version.
- NuGet.
- MSBuild.
- MySQL running locally.
- Required environment variables and application configuration.

### Development Commands

The following commands use `SumaqAgro.sln` as an example. Replace it with the actual solution filename.

```powershell
# Restore NuGet packages
nuget restore SumaqAgro.sln

# Build the application in Debug mode
msbuild SumaqAgro.sln /p:Configuration=Debug

# Run the application
# Open SumaqAgro.sln in Visual Studio
# Press F5 to start the application using IIS Express

# Run the tests
# Replace the path with the compiled test assembly
vstest.console.exe path\to\Tests.dll

# Build the application for production
msbuild SumaqAgro.sln /p:Configuration=Release
```

The application URL and port depend on the IIS Express settings configured in Visual Studio.

### How to Check a Change

1. Build and run the application using Visual Studio.
2. Verify that Entity Framework mappings and database tables are correct.
3. Test the RESTful endpoints using Swagger UI or an API testing tool.
4. Verify successful requests and expected error responses.
5. Inspect MySQL tables to confirm persisted data.
6. When connected to the Vue.js Web Application, inspect HTTP requests in the browser's Network tab.
7. Confirm that the JSON response matches the frontend contract.

### Tests

- Test aggregate rules, value objects and assemblers with unit tests.
- Keep domain unit tests independent of Entity Framework and MySQL.
- Use a testing framework compatible with the project's .NET Framework version.
- Name test classes `<ClassName>Tests`.
- Include positive and negative test cases.
- Verify validation failures and business rule violations.

---

## Pull Request (PR) Process

Before merging into `develop`, verify that:

1. [ ] The code respects the DDD layers and dependency rules.
2. [ ] Commit messages follow Conventional Commits.
3. [ ] Every new message key exists in English and Spanish.
4. [ ] The application builds successfully using MSBuild and all tests pass.
5. [ ] RESTful endpoints were tested, including error cases.
6. [ ] No secret, API key or password was committed.
7. [ ] JSON responses match the contract used by the Vue.js Web Application.
8. [ ] Changes in `Shared` were announced to the team.
9. [ ] `CHANGELOG.md` is updated when features or architecture change.
10. [ ] The branch targets `develop`, or `main` for hotfixes.
