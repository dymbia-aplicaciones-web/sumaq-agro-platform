# Certification and Traceability

Manages harvest batches (`HarvestBatch`) and quality certificates (`QualityCertificate`) for Andean potatoes and specialty coffee.

## Business rules

The member must belong to the cooperative before registering a batch. Weigh and sample the batch before grading potatoes by size or cupping coffee. A certificate can only be issued for an eligible batch, and each batch can have only one certificate. Public verification uses the certificate number and hash.

## Endpoints

| Method | Route | Operation |
| --- | --- | --- |
| `POST` | `/api/v1/harvest-batches` | Register a batch |
| `GET` | `/api/v1/harvest-batches/{harvestBatchId}` | Get a batch |
| `POST` | `/api/v1/harvest-batches/{harvestBatchId}/weight` | Record batch weight |
| `POST` | `/api/v1/harvest-batches/{harvestBatchId}/sample` | Take a sample |
| `POST` | `/api/v1/harvest-batches/{harvestBatchId}/potato-calibers` | Grade potatoes by size |
| `POST` | `/api/v1/harvest-batches/{harvestBatchId}/coffee-cupping` | Record coffee cupping results |
| `POST` | `/api/v1/quality-certificates` | Issue a certificate |
| `GET` | `/api/v1/quality-certificates?certificateNumber={number}` | Get a certificate |
| `POST` | `/api/v1/quality-certificates/{qualityCertificateId}/revocation` | Revoke a certificate |
| `GET` | `/api/v1/public/traceability?number={number}&hash={hash}` | Verify public traceability |

## Persistence

Entity Framework Core manages these MySQL tables: `harvest_batches`, `quality_certificates`.

## Local testing

Use a JWT to manage batches and certificates. Public verification is anonymous. Use one batch per crop type and keep the certificate number and hash returned when you issue it.

Run the [certification.http](../../SumaqAgro.Platform/Requests/certification.http) requests in Rider or use [Scalar](http://localhost:5192/scalar/v1) while the API is running with the local launch profile.
