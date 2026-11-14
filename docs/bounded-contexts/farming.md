# Parcelas y campañas agrícolas

Gestiona `FieldPlot` y `CropCampaign`, desde el registro de la parcela hasta el cierre de la campaña.

## Reglas de negocio

El registro valida que el nombre de parcela no esté repetido para el propietario y comprueba su cuota. Una parcela admite una campaña activa a la vez. Las campañas admiten papa andina (`ANDEAN_POTATO`) y café de especialidad (`SPECIALTY_COFFEE`). La delimitación valida el polígono e integra AgroMonitoring mediante `IPolygonMonitoring`.

## Endpoints

| Método | Ruta | Operación |
| --- | --- | --- |
| `POST` | `/api/v1/field-plots` | Registrar parcela |
| `GET` | `/api/v1/field-plots?ownerUserId={id}` | Listar parcelas por propietario |
| `GET` | `/api/v1/field-plots/{fieldPlotId}` | Consultar parcela |
| `PUT` | `/api/v1/field-plots/{fieldPlotId}/boundary` | Delimitar parcela |
| `GET` | `/api/v1/field-plots/{fieldPlotId}/crop-campaigns` | Listar campañas de la parcela |
| `POST` | `/api/v1/crop-campaigns` | Iniciar campaña |
| `GET` | `/api/v1/crop-campaigns/{cropCampaignId}` | Consultar campaña |
| `PUT` | `/api/v1/crop-campaigns/{cropCampaignId}/crop-type` | Cambiar cultivo |
| `PUT` | `/api/v1/crop-campaigns/{cropCampaignId}/seed-variety` | Cambiar variedad de semilla |
| `PUT` | `/api/v1/crop-campaigns/{cropCampaignId}/sowing-date` | Registrar fecha de siembra |
| `POST` | `/api/v1/crop-campaigns/{cropCampaignId}/closures` | Cerrar campaña |

## Persistencia

Entity Framework Core administra en MySQL las tablas `field_plots`, `crop_campaigns`.

## Prueba local

Usa un JWT y registra una parcela antes de iniciar una campaña. Para probar la delimitación configura `AgroMonitoring:ApiKey` o `AGROMONITORING_API_KEY`.

Ejecuta las solicitudes de [farming.http](../../SumaqAgro.Platform/Requests/farming.http) desde Rider o usa [Scalar](http://localhost:5192/scalar/v1) con la API ejecutándose en el perfil local.
