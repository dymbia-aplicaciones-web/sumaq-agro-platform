using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SumaqAgro.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;

public sealed class DateOnlyConverter() : ValueConverter<DateOnly, DateTime>(
    value => value.ToDateTime(TimeOnly.MinValue),
    value => DateOnly.FromDateTime(value));
