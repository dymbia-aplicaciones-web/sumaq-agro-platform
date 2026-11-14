namespace SumaqAgro.Platform.Subscriptions.Domain.Model.Entities;

public sealed record Plan(string Code, decimal MonthlyPrice, decimal AnnualPrice, int RegisteredPlotLimit,
    int ProducerLimit, int SupervisedPlotLimit);
