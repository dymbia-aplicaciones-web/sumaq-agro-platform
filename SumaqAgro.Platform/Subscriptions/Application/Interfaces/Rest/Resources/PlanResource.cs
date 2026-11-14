using System.ComponentModel.DataAnnotations;

namespace SumaqAgro.Platform.Subscriptions.Interfaces.Rest.Resources;

public sealed record PlanResource(string Code, decimal MonthlyPrice, decimal AnnualPrice, string Currency,
    int RegisteredPlotLimit, int ProducerLimit, int SupervisedPlotLimit);
