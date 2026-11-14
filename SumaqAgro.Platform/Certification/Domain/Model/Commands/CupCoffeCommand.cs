namespace SumaqAgro.Platform.Certification.Domain.Model.Commands;

public sealed record CupCoffeeCommand(long Id, decimal Score, bool FermentDefect);