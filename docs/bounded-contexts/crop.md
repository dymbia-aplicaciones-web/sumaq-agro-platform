# Crop Health and Monitoring

Manages `PestReport`, `FieldInspection`, `TechnicalPrescription`, and `RegionalBulletin`. It also exposes queries for `SatelliteObservation`, `ClimateForecast`, and `AgroclimaticAlert`.

## Business rules

A pending report can be scheduled for inspection at a future time. Completing the inspection at or after the scheduled time marks the report as `INSPECTED`. A prescription requires an inspected report, and its application can be recorded once. Monitoring queries read stored records.

## Endpoints

| Method | Route | Operation |
| --- | --- | --- |
| `POST` | `/api/v1/pest-reports` | Submit a pest report |
| `GET` | `/api/v1/pest-reports?plotId={id}` | List reports by plot |
| `GET` | `/api/v1/pest-reports/{pestReportId}` | Get a report |
| `DELETE` | `/api/v1/pest-reports/{pestReportId}` | Delete a report |
| `POST` | `/api/v1/field-inspections` | Schedule an inspection |
| `GET` | `/api/v1/field-inspections?plotId={id}` | List inspections by plot |
| `GET` | `/api/v1/field-inspections/{fieldInspectionId}` | Get an inspection |
| `PATCH` | `/api/v1/field-inspections/{fieldInspectionId}/completion` | Complete an inspection |
| `POST` | `/api/v1/technical-prescriptions` | Issue a prescription |
| `GET` | `/api/v1/technical-prescriptions?plotId={id}` | List prescriptions by plot |
| `GET` | `/api/v1/technical-prescriptions/{technicalPrescriptionId}` | Get a prescription |
| `PATCH` | `/api/v1/technical-prescriptions/{technicalPrescriptionId}/application` | Record a prescription application |
| `POST` | `/api/v1/regional-bulletins` | Publish a bulletin |
| `GET` | `/api/v1/regional-bulletins?region={region}` | List bulletins by region |
| `GET` | `/api/v1/satellite-observations?plotId={id}` | List satellite observations by plot |
| `GET` | `/api/v1/climate-forecasts?region={region}` | List forecasts by region |
| `GET` | `/api/v1/agroclimatic-alerts?region={region}` | List alerts by region |

## Persistence

Entity Framework Core manages these MySQL tables: `pest_reports`, `field_inspections`, `technical_prescriptions`, `regional_bulletins`, `satellite_observations`, `climate_forecasts`, `agroclimatic_alerts`.

## Local testing

Use a JWT and a plot ID from Farming. Submit a report, complete an inspection, issue a prescription, and record its application. Use a separate report to test deletion.

Run the [crop.http](../../SumaqAgro.Platform/Requests/crop.http) requests in Rider or use [Scalar](http://localhost:5192/scalar/v1) while the API is running with the local launch profile.
