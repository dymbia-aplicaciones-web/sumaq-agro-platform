using Microsoft.AspNetCore.WebUtilities;
using SumaqAgro.Platform.Crop.Application.Internal.OutboundServices;

namespace SumaqAgro.Platform.Crop.Infrastructure.Acl;

public sealed class AgroMonitoringSatelliteImagery(HttpClient client, IConfiguration configuration) : ISatelliteImagery
{
    public async Task<SatelliteMeasurement?> FindLatestAsync(string polygonId, long start, long end, CancellationToken ct)
    {
        var key = configuration["AgroMonitoring:ApiKey"] ?? configuration["AGROMONITORING_API_KEY"];
        if (string.IsNullOrWhiteSpace(key)) throw new HttpRequestException("Satellite provider is not configured.");

        var path = QueryHelpers.AddQueryString("image/search", new Dictionary<string, string?>
        {
            ["polyid"] = polygonId, ["start"] = start.ToString(System.Globalization.CultureInfo.InvariantCulture),
            ["end"] = end.ToString(System.Globalization.CultureInfo.InvariantCulture), ["appid"] = key
        });
        var scenes = await client.GetFromJsonAsync<Scene[]>(path, ct) ?? [];
        foreach (var scene in scenes.Where(s => s is not null && s.Dt >= start && s.Dt <= end &&
                     !string.IsNullOrWhiteSpace(s.Type) && s.Cl is >= 0 and <= 100 && s.Stats is not null)
                     .OrderByDescending(s => s.Dt).ThenBy(s => s.Type, StringComparer.Ordinal))
        {
            var ndvi = await ReadMeanAsync(scene.Stats!.Ndvi, polygonId, key, ct);
            var ndwi = await ReadMeanAsync(scene.Stats.Ndwi, polygonId, key, ct);
            if (ndvi is not null || ndwi is not null)
                return new(scene.Dt, scene.Type!, ndvi, ndwi, scene.Cl!.Value);
        }
        return null;
    }

    private async Task<double?> ReadMeanAsync(string? link, string polygonId, string key, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(link)) return null;
        if (!Uri.TryCreate(link, UriKind.Absolute, out var uri) ||
            !string.Equals(uri.Host, client.BaseAddress!.Host, StringComparison.OrdinalIgnoreCase) ||
            !System.Text.RegularExpressions.Regex.IsMatch(uri.AbsolutePath,
                "^/stats/1[.]0/[A-Za-z0-9_-]+/" + System.Text.RegularExpressions.Regex.Escape(polygonId) + "$"))
            throw new HttpRequestException("Invalid satellite statistics link.");

        var stats = await client.GetFromJsonAsync<Statistics>(
            QueryHelpers.AddQueryString(uri.AbsolutePath, "appid", key), ct);
        return stats?.Mean is >= -1 and <= 1 ? stats.Mean : null;
    }

    private sealed record Scene(long Dt, string? Type, double? Cl, Links? Stats);
    private sealed record Links(string? Ndvi, string? Ndwi);
    private sealed record Statistics(double? Mean);
}
