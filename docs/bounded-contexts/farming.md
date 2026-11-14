# Field Plots and Crop Campaigns

Manages `FieldPlot` and `CropCampaign`, from plot registration to campaign closure.

## Business rules

Plot names must be unique per owner, and registration checks the owner's plot quota. Each plot can have one active campaign at a time. Campaigns support Andean potatoes (`ANDEAN_POTATO`) and specialty coffee (`SPECIALTY_COFFEE`). Boundary updates validate the polygon and use `IPolygonMonitoring` to integrate with AgroMonitoring.

## Endpoints

| Method | Route | Operation |
| --- | --- | --- |
| `POST` | `/api/v1/field-plots` | Register a plot |
| `GET` | `/api/v1/field-plots?ownerUserId={id}` | List plots by owner |
| `GET` | `/api/v1/field-plots/{fieldPlotId}` | Get a plot |
| `PUT` | `/api/v1/field-plots/{fieldPlotId}/boundary` | Set the plot boundary |
| `GET` | `/api/v1/field-plots/{fieldPlotId}/crop-campaigns` | List campaigns for a plot |
| `POST` | `/api/v1/crop-campaigns` | Start a campaign |
| `GET` | `/api/v1/crop-campaigns/{cropCampaignId}` | Get a campaign |
| `PUT` | `/api/v1/crop-campaigns/{cropCampaignId}/crop-type` | Change the crop type |
| `PUT` | `/api/v1/crop-campaigns/{cropCampaignId}/seed-variety` | Change the seed variety |
| `PUT` | `/api/v1/crop-campaigns/{cropCampaignId}/sowing-date` | Record the sowing date |
| `POST` | `/api/v1/crop-campaigns/{cropCampaignId}/closures` | Close a campaign |

## Persistence

Entity Framework Core manages these MySQL tables: `field_plots`, `crop_campaigns`.

## Local testing

Use a JWT and register a plot before starting a campaign. To test boundary registration, configure `AgroMonitoring:ApiKey` or `AGROMONITORING_API_KEY`.

Run the [farming.http](../../SumaqAgro.Platform/Requests/farming.http) requests in Rider or use [Scalar](http://localhost:5192/scalar/v1) while the API is running with the local launch profile.
