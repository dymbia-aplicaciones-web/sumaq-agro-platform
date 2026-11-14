using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SumaqAgro.Platform.Iam.Domain.Model.Aggregates;
using SumaqAgro.Platform.Subscriptions.Domain.Model.Aggregates;
using SumaqAgro.Platform.Subscriptions.Domain.Model.Entities;

namespace SumaqAgro.Platform.Subscriptions.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.HasKey(plan => plan.Code);
        builder.Property(plan => plan.Code).HasMaxLength(32);
        builder.HasData(
            new Plan("SEED", 49m, 490m, 4, 0, 0),
            new Plan("COOPERATIVE_PRO", 189m, 1890m, 50, 50, 0),
            new Plan("TECHNICAL_ADVISOR", 89m, 890m, 0, 0, 20));
    }
}

public sealed class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasOne<User>().WithOne().HasForeignKey<Subscription>(subscription => subscription.UserId);
        builder.HasOne<Plan>().WithMany().HasForeignKey(subscription => subscription.PlanCode);
    }
}

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(payment => payment.Reference);
        builder.Property(payment => payment.Reference).HasMaxLength(41);
        builder.HasOne(payment => payment.Subscription).WithMany().HasForeignKey(payment => payment.SubscriptionId);
        builder.HasOne<User>().WithMany().HasForeignKey(payment => payment.UserId);
        builder.HasOne<Plan>().WithMany().HasForeignKey(payment => payment.PlanCode);
    }
}
