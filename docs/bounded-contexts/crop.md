# Sanidad y monitoreo de cultivos

Gestiona `PestReport`, `FieldInspection`, `TechnicalPrescription` y `RegionalBulletin`. Expone consultas de `SatelliteObservation`, `ClimateForecast` y `AgroclimaticAlert`.

## Reglas de negocio

Se programa una inspección futura sobre un reporte pendiente. Completar la inspección a partir de la hora programada marca el reporte como `INSPECTED`. Emitir una prescripción requiere un reporte inspeccionado. La aplicación se registra una vez. Las consultas de monitoreo leen los registros persistidos.

## Endpoints

| Método | Ruta | Operación |
| --- | --- | --- |
| `POST` | `/api/v1/pest-reports` | Registrar reporte |
| `GET` | `/api/v1/pest-reports?plotId={id}` | Listar reportes por parcela |
| `GET` | `/api/v1/pest-reports/{pestReportId}` | Consultar reporte |
| `DELETE` | `/api/v1/pest-reports/{pestReportId}` | Eliminar reporte |
| `POST` | `/api/v1/field-inspections` | Programar inspección |
| `GET` | `/api/v1/field-inspections?plotId={id}` | Listar inspecciones por parcela |
| `GET` | `/api/v1/field-inspections/{fieldInspectionId}` | Consultar inspección |
| `PATCH` | `/api/v1/field-inspections/{fieldInspectionId}/completion` | Completar inspección |
| `POST` | `/api/v1/technical-prescriptions` | Emitir prescripción |
| `GET` | `/api/v1/technical-prescriptions?plotId={id}` | Listar prescripciones por parcela |
| `GET` | `/api/v1/technical-prescriptions/{technicalPrescriptionId}` | Consultar prescripción |
| `PATCH` | `/api/v1/technical-prescriptions/{technicalPrescriptionId}/application` | Registrar aplicación |
| `POST` | `/api/v1/regional-bulletins` | Publicar boletín |
| `GET` | `/api/v1/regional-bulletins?region={region}` | Listar boletines por región |
| `GET` | `/api/v1/satellite-observations?plotId={id}` | Listar observaciones por parcela |
| `GET` | `/api/v1/climate-forecasts?region={region}` | Listar pronósticos por región |
| `GET` | `/api/v1/agroclimatic-alerts?region={region}` | Listar alertas por región |

## Persistencia

Entity Framework Core administra en MySQL las tablas `pest_reports`, `field_inspections`, `technical_prescriptions`, `regional_bulletins`, `satellite_observations`, `climate_forecasts`, `agroclimatic_alerts`.

## Prueba local

Usa un JWT y el ID de una parcela de Farming. Prueba el flujo reporte, inspección, prescripción y aplicación en ese orden. Usa un reporte separado para probar la eliminación.

Ejecuta las solicitudes de [crop.http](../../SumaqAgro.Platform/Requests/crop.http) desde Rider o usa [Scalar](http://localhost:5192/scalar/v1) con la API ejecutándose en el perfil local.
