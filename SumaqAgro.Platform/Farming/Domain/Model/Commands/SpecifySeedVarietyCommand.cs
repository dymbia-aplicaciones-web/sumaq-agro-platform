namespace SumaqAgro.Platform.Farming.Domain.Model.Commands;

public sealed record SpecifySeedVarietyCommand(long Id, string Seed, bool Custom);
