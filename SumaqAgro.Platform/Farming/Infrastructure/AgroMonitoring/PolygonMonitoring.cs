using System.Net.Http.Json;
using System.Text.Json;
using SumaqAgro.Platform.Farming.Application.Internal.OutboundServices;
using SumaqAgro.Platform.Farming.Domain.Model.Aggregates;
using SumaqAgro.Platform.Farming.Domain.Model.ValueObjects;

namespace SumaqAgro.Platform.Farming.Infrastructure.AgroMonitoring;

public sealed class PolygonMonitoring(HttpClient client, IConfiguration configuration, ILogger<PolygonMonitoring> logger)
    : IPolygonMonitoring
{
    public async Task<string?> ReplaceAsync(string? previousId, string name, PlotBoundary boundary,
        CancellationToken cancellationToken)
    {
        var key = configuration["AgroMonitoring:ApiKey"] ?? configuration["AGROMONITORING_API_KEY"];
        if (string.IsNullOrWhiteSpace(key)) return null;

        var query = "?appid=" + Uri.EscapeDataString(key);
        if (previousId is not null)
        {
            try
            {
                using var removed = await client.DeleteAsync("polygons/" + Uri.EscapeDataString(previousId) + query,
                    cancellationToken);
                if (!removed.IsSuccessStatusCode)
                    logger.LogWarning("AgroMonitoring returned {StatusCode} while removing the previous polygon.", removed.StatusCode);
            }
            catch (Exception exception) when (exception is HttpRequestException or OperationCanceledException &&
                                              !cancellationToken.IsCancellationRequested)
            {
                logger.LogWarning("AgroMonitoring could not remove the previous polygon.");
            }
        }

        try
        {
            using var response = await client.PostAsJsonAsync("polygons" + query, new
            {
                name,
                geo_json = new
                {
                    type = "Feature",
                    properties = new Dictionary<string, string>(),
                    geometry = new { type = "Polygon", coordinates = new[] { boundary.ToGeoJsonRing() } }
                }
            }, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var document = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
                return document.TryGetProperty("id", out var id) && id.ValueKind == JsonValueKind.String
                    ? id.GetString()
                    : null;
            }

            logger.LogWarning("AgroMonitoring rejected the polygon with status {StatusCode}.", response.StatusCode);
        }
        catch (Exception exception) when (exception is HttpRequestException or JsonException or OperationCanceledException &&
                                          !cancellationToken.IsCancellationRequested)
        {
            logger.LogWarning("AgroMonitoring could not register the polygon.");
        }

        return null;
    }
}
