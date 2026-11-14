# Sanidad y monitoreo de cultivos

Gestiona reportes, inspecciones, prescripciones y consultas de monitoreo.

## Modelo de dominio

| Elemento | Responsabilidad |
| --- | --- |
| `PestReport` | reporte |
| `FieldInspection` | inspección |
| `TechnicalPrescription` | prescripción |
| `RegionalBulletin` | boletín |
| `SatelliteObservation` | observación |
| `ClimateForecast` | pronóstico |
| `AgroclimaticAlert` | alerta. |

## Reglas de negocio

- Una inspección se programa para un reporte pendiente y se completa a partir de la hora programada. Completarla marca el reporte como INSPECTED. La prescripción requiere un reporte inspeccionado y su aplicación se registra una vez. Las consultas de monitoreo leen datos persistidos.

## Endpoints

| Método | Ruta | Descripción |
| --- | --- | --- |
| POST | `/api/v1/pest-reports` | Registrar reporte |
| GET | `/api/v1/pest-reports?plotId={id}` | Listar reportes |
| GET | `/api/v1/pest-reports/{pestReportId}` | Consultar reporte |
| DELETE | `/api/v1/pest-reports/{pestReportId}` | Eliminar reporte |
| POST | `/api/v1/field-inspections` | Programar inspección |
| GET | `/api/v1/field-inspections?plotId={id}` | Listar inspecciones |
| GET | `/api/v1/field-inspections/{fieldInspectionId}` | Consultar inspección |
| PATCH | `/api/v1/field-inspections/{fieldInspectionId}/completion` | Completar inspección |
| POST | `/api/v1/technical-prescriptions` | Emitir prescripción |
| GET | `/api/v1/technical-prescriptions?plotId={id}` | Listar prescripciones |
| GET | `/api/v1/technical-prescriptions/{technicalPrescriptionId}` | Consultar prescripción |
| PATCH | `/api/v1/technical-prescriptions/{technicalPrescriptionId}/application` | Registrar aplicación |
| POST | `/api/v1/regional-bulletins` | Publicar boletín |
| GET | `/api/v1/regional-bulletins?region={region}` | Listar boletines |
| GET | `/api/v1/satellite-observations?plotId={id}` | Listar observaciones |
| GET | `/api/v1/climate-forecasts?region={region}` | Listar pronósticos |
| GET | `/api/v1/agroclimatic-alerts?region={region}` | Listar alertas |

Los endpoints requieren `Authorization: Bearer <token>`.

## Persistencia

Entity Framework Core administra las tablas `pest_reports`, `field_inspections`, `technical_prescriptions`, `regional_bulletins`, `satellite_observations`, `climate_forecasts`, `agroclimatic_alerts` en MySQL.

## Pruebas locales

Con la API en ejecución, usa [crop.http](../../SumaqAgro.Platform/Requests/crop.http) desde Rider o [Scalar](http://localhost:5192/scalar/v1).
