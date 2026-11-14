# SumaqAgro Platform

SumaqAgro is an agricultural management platform developed by **Dymbia** for Peruvian farmers, cooperatives, and agronomists. It focuses on Andean potato and specialty coffee production. This repository contains the C# REST API for the **Aplicaciones Web** course, adapted from the project's [Java implementation for the Open Source course](https://github.com/dymbia-opensource/sumaq-agro-platform).

| Academic information | Details |
| --- | --- |
| University | Universidad Peruana de Ciencias Aplicadas (UPC) |
| Course | 1ASI0730 Aplicaciones Web (Web Applications) |
| Section | NRC 8130 |
| Academic term | 2026-20 |
| Instructor | Óscar Iván Villafuerte Bazán |
| Team | Dymbia |

## Backend

The API uses ASP.NET Core and C# on .NET 10, Entity Framework Core with MySQL, and JWT bearer authentication. OpenAPI describes the HTTP endpoints, and Scalar provides an interactive interface for exploring them.

The backend groups its code into six bounded contexts. Each context organizes its business logic, application services, persistence, and REST interfaces by responsibility. Common components live in `SumaqAgro.Platform/Shared`.

| Bounded context | Responsibilities |
| --- | --- |
| [IAM](docs/bounded-contexts/iam.md) | User registration, password hashing, and JWT issuance |
| [Profiles](docs/bounded-contexts/profiles.md) | User profiles, cooperatives, memberships, and agronomist assignments |
| [Farming](docs/bounded-contexts/farming.md) | Field plots, boundaries, and crop campaigns |
| [Crop](docs/bounded-contexts/crop.md) | Pest reports, field inspections, prescriptions, and monitoring queries |
| [Certification](docs/bounded-contexts/certification.md) | Harvest batches, quality certificates, and public traceability |
| [Subscriptions](docs/bounded-contexts/subscriptions.md) | Plans, subscriptions, and simulated checkout |

## Run locally

Install the **.NET 10 SDK** and start a **MySQL server**. From the repository root, configure your development connection using .NET User Secrets. Replace the example values with your local MySQL credentials.

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=127.0.0.1;Port=3306;Database=sumaqagro;User ID=YOUR_MYSQL_USER;Password=YOUR_MYSQL_PASSWORD;" --project SumaqAgro.Platform
dotnet build
dotnet run --project SumaqAgro.Platform --launch-profile SumaqAgro.Platform
```

The launch profile sets the environment to `Development` and starts the API on port `5192`. At startup, EF Core applies pending migrations using the configured connection. The MySQL account needs permission to create or update the schema.

To register field boundaries with AgroMonitoring locally, set `AgroMonitoring:ApiKey` in User Secrets. This key is needed for the external polygon integration.

## Try the API

- Open [Scalar](http://localhost:5192/scalar/v1) or download the [OpenAPI document](http://localhost:5192/openapi/v1.json).
- Register a test user and sign in with the [IAM request samples](SumaqAgro.Platform/Requests/iam.http).
- Send the returned token as `Authorization: Bearer <token>` when calling protected endpoints. The [Requests folder](SumaqAgro.Platform/Requests) contains samples for all six bounded contexts.

## Project links

- [Final project report](https://github.com/dymbia-aplicaciones-web/sumaq-agro-report)
- [SumaqAgro landing page repository](https://github.com/dymbia-aplicaciones-web/sumaq-agro-website)
- [Contribution guidelines](CONTRIBUTING.md)
- [Changelog](CHANGELOG.md)
- [MIT License](LICENSE.md)
