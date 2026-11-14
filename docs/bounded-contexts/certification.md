# Certificación y trazabilidad

Gestiona `HarvestBatch` y `QualityCertificate`. El flujo registra lotes de papa andina o café de especialidad, evalúa su calidad y permite consultar su trazabilidad.

## Reglas de negocio

Al registrar un lote se comprueba que el miembro pertenezca a la cooperativa. El lote se pesa y se muestrea antes de clasificar la papa por calibres o catar el café. La emisión valida el estado del lote y evita emitir un segundo certificado para el mismo lote. La verificación pública recibe el número y el hash del certificado.

## Endpoints

| Método | Ruta | Operación |
| --- | --- | --- |
| `POST` | `/api/v1/harvest-batches` | Registrar lote |
| `GET` | `/api/v1/harvest-batches/{harvestBatchId}` | Consultar lote |
| `POST` | `/api/v1/harvest-batches/{harvestBatchId}/weight` | Registrar peso |
| `POST` | `/api/v1/harvest-batches/{harvestBatchId}/sample` | Tomar muestra |
| `POST` | `/api/v1/harvest-batches/{harvestBatchId}/potato-calibers` | Clasificar papa |
| `POST` | `/api/v1/harvest-batches/{harvestBatchId}/coffee-cupping` | Registrar catación de café |
| `POST` | `/api/v1/quality-certificates` | Emitir certificado |
| `GET` | `/api/v1/quality-certificates?certificateNumber={number}` | Consultar certificado |
| `POST` | `/api/v1/quality-certificates/{qualityCertificateId}/revocation` | Revocar certificado |
| `GET` | `/api/v1/public/traceability?number={number}&hash={hash}` | Verificar trazabilidad pública |

## Persistencia

Entity Framework Core administra en MySQL las tablas `harvest_batches`, `quality_certificates`.

## Prueba local

Usa JWT para gestionar lotes y certificados. La verificación pública admite solicitudes sin JWT. Usa un lote por cada tipo de cultivo y conserva el número y el hash devueltos por la emisión.

Ejecuta las solicitudes de [certification.http](../../SumaqAgro.Platform/Requests/certification.http) desde Rider o usa [Scalar](http://localhost:5192/scalar/v1) con la API ejecutándose en el perfil local.
