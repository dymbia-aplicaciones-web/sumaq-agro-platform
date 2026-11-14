namespace SumaqAgro.Platform.Farming.Domain.Model.ValueObjects;

public sealed record GeoCoordinate(double Latitude, double Longitude);

public sealed record PlotBoundary(GeoCoordinate[] Vertices, decimal Area)
{
    public static PlotBoundary Create(GeoCoordinate[] ring)
    {
        if (ring.Length > 1 && ring[0] == ring[^1])
            ring = ring[..^1];

        if (ring.Distinct().Count() < 3 || ring.Any(point => point is null ||
            !double.IsFinite(point.Latitude) || !double.IsFinite(point.Longitude) ||
            Math.Abs(point.Latitude) > 90 || Math.Abs(point.Longitude) > 180))
            throw new ArgumentException("Provide at least three distinct valid coordinates.");

        for (var i = 0; i < ring.Length; i++)
        for (var j = i + 2; j < ring.Length; j++)
        {
            if (i == 0 && j == ring.Length - 1) continue;
            if (Intersects(ring[i], ring[(i + 1) % ring.Length], ring[j], ring[(j + 1) % ring.Length]))
                throw new ArgumentException("The plot boundary intersects itself.");
        }

        const double metersPerDegree = Math.PI * 6_371_008.8 / 180;
        var longitudeScale = metersPerDegree * Math.Cos(ring.Average(point => point.Latitude) * Math.PI / 180);
        var origin = ring[0];
        var signedArea = 0d;
        for (var i = 0; i < ring.Length; i++)
        {
            var a = ring[i];
            var b = ring[(i + 1) % ring.Length];
            signedArea += (a.Longitude - origin.Longitude) * (b.Latitude - origin.Latitude) -
                          (b.Longitude - origin.Longitude) * (a.Latitude - origin.Latitude);
        }

        var hectares = Math.Round((decimal)(Math.Abs(signedArea) * longitudeScale * metersPerDegree / 20_000),
            2, MidpointRounding.AwayFromZero);
        if (hectares is < 1 or > 3000)
            throw new ArgumentException("The boundary area must be between 1 and 3000 hectares.");

        return new PlotBoundary(ring, hectares);
    }

    public double[][] ToGeoJsonRing()
    {
        var signedArea = 0d;
        for (var i = 0; i < Vertices.Length; i++)
        {
            var a = Vertices[i];
            var b = Vertices[(i + 1) % Vertices.Length];
            signedArea += a.Longitude * b.Latitude - b.Longitude * a.Latitude;
        }
        var points = signedArea > 0 ? Vertices : Vertices.Reverse().ToArray();
        return points.Append(points[0]).Select(point => new[] { point.Longitude, point.Latitude }).ToArray();
    }

    private static bool Intersects(GeoCoordinate a, GeoCoordinate b, GeoCoordinate c, GeoCoordinate d)
    {
        var abC = Orientation(a, b, c);
        var abD = Orientation(a, b, d);
        var cdA = Orientation(c, d, a);
        var cdB = Orientation(c, d, b);
        return (abC * abD < 0 && cdA * cdB < 0) ||
               (abC == 0 && Contains(a, b, c)) || (abD == 0 && Contains(a, b, d)) ||
               (cdA == 0 && Contains(c, d, a)) || (cdB == 0 && Contains(c, d, b));
    }

    private static int Orientation(GeoCoordinate a, GeoCoordinate b, GeoCoordinate c) =>
        Math.Sign((b.Longitude - a.Longitude) * (c.Latitude - a.Latitude) -
                  (b.Latitude - a.Latitude) * (c.Longitude - a.Longitude));

    private static bool Contains(GeoCoordinate a, GeoCoordinate b, GeoCoordinate point) =>
        point.Longitude >= Math.Min(a.Longitude, b.Longitude) && point.Longitude <= Math.Max(a.Longitude, b.Longitude) &&
        point.Latitude >= Math.Min(a.Latitude, b.Latitude) && point.Latitude <= Math.Max(a.Latitude, b.Latitude);
}
