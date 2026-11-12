# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

The SumaqAgro RESTful API is currently under development. The Web Application is built with Vue.js, while the backend uses C# and .NET Framework.

### Added

- **Project Setup**:
    - Frontend application developed with Vue.js.
    - Backend RESTful API developed with C# and ASP.NET Web API on .NET Framework.
    - Entity Framework for database persistence and MySQL as the relational database.
    - Application configuration using `Web.config` for database connections and external API credentials.
    - Multilingual support for English and Spanish.
    - Project documentation: `CHANGELOG.md`, `CONTRIBUTING.md` and the MIT `LICENSE.md`.

- **Shared Kernel**:
    - Generic `Result<T, E>` class for handling successful and failed operations.
    - `ApplicationError` for standardized error handling.
    - `AbstractDomainAggregateRoot` for managing domain aggregates.
    - `AuditableBaseEntity` with `Id`, `CreatedAt` and `UpdatedAt`.
    - `DomainEventPublisher` for publishing domain events.
    - Standardized API response models.
    - Global exception handling for validation and business rule errors.
    - Response assemblers for consistent HTTP responses.

- **Frontend – Vue.js**:
    - Component-based architecture for reusable interface elements.
    - Vue Router for application navigation.
    - Axios for RESTful API communication.
    - Integration with json-server as a temporary fake API.
    - Forms and views for agricultural field management.
    - Form validation and API error handling.

- **Farming Bounded Context** (Field Management):
    - `FieldPlot` aggregate with `PlotStatus`.
    - Commands and queries for field plot management.
    - Repository interfaces and application services.
    - RESTful endpoints:
        - `POST /api/v1/field-plots`
        - `GET /api/v1/field-plots/{id}`
    - Business rules preventing duplicate plot names.
    - Seed plan quota validation with a maximum of 4 plots.

- **Crop Health Bounded Context**:
    - Aggregates `PestReport`, `SatelliteObservation`, `AgroclimaticAlert` and `ClimateForecast`.
    - Commands and queries for crop health monitoring.
    - Value objects for NDVI/NDWI indexes, percentages, severity levels and statuses.
    - Entity Framework mappings for database persistence.

### Changed

- **Backend**: Established C# and ASP.NET Web API on .NET Framework as the backend architecture.
- **Frontend**: Established Vue.js as the frontend framework.
- **Persistence**: Implemented Entity Framework for database operations.
- **Configuration**: Standardized backend configuration using `Web.config`.
- **Farming**: Standardized the bounded context name as `Farming`.
- **Field Plot**: Aligned the Seed plan quota with the Web Application (4 plots).
- **Field Plot**: Implemented transaction management through Entity Framework.
- **API Controllers**: Standardized HTTP responses and error handling.

### Fixed

- **Shared**: Validation exceptions return `400 Bad Request` instead of `500 Internal Server Error`.
- **API**: Improved handling of invalid requests and business rule violations.
- **Frontend**: Improved API error handling and form validation in Vue.js components.