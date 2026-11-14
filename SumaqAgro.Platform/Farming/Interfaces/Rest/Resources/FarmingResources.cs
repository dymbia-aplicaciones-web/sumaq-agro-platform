using System.ComponentModel.DataAnnotations;
using SumaqAgro.Platform.Farming.Domain.Model.Aggregates;
using SumaqAgro.Platform.Farming.Domain.Model.ValueObjects;

namespace SumaqAgro.Platform.Farming.Interfaces.Rest.Resources;

public sealed record RegisterFieldPlotResource(
    [param: Range(1, long.MaxValue)] long OwnerUserId,
    [param: Required, StringLength(100)] string Name,
    [param: Required, StringLength(150)] string Region,
    [param: Required, Range(typeof(decimal), "0", "99999999.99")] decimal? AreaHectares);

public sealed record CoordinateResource(
    [param: Required, Range(-90d, 90d)] double? Latitude,
    [param: Required, Range(-180d, 180d)] double? Longitude);

public sealed record DelineatePlotBoundaryResource([param: Required] CoordinateResource?[] Vertices);

public sealed record StartCropCampaignResource(
    [param: Range(1, long.MaxValue)] long FieldPlotId,
    [param: Required, RegularExpression(CropSeason.Pattern)] string Season,
    [param: Required, RegularExpression(@"(?i)^\s*(ANDEAN_POTATO|SPECIALTY_COFFEE)\s*$")] string CropType,
    [param: Required, StringLength(60)] string SeedVariety,
    bool CustomSeedVariety, DateOnly? SowingDate);

public sealed record SelectCropTypeResource(
    [param: Required, RegularExpression(@"(?i)^\s*(ANDEAN_POTATO|SPECIALTY_COFFEE)\s*$")] string CropType);

public sealed record SpecifySeedVarietyResource([param: Required, StringLength(60)] string SeedVariety, bool CustomSeedVariety);
public sealed record RecordSowingDateResource([param: Required] DateOnly? SowingDate);

public sealed record FieldPlotResource(long Id, long OwnerUserId, string Name, string Region, string Status,
    decimal AreaHectares, string? AgroMonitoringPolygonId, GeoCoordinate[] Boundary);
