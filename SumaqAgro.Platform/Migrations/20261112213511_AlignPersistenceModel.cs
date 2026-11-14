using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SumaqAgro.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AlignPersistenceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_agronomist_invitations__cooperative_cooperative_id",
                table: "agronomist_invitations");

            migrationBuilder.DropForeignKey(
                name: "f_k_payments__subscription_subscription_id",
                table: "payments");

            migrationBuilder.DropForeignKey(
                name: "f_k_subscriptions_plans_plan_code",
                table: "subscriptions");

            migrationBuilder.AddForeignKey(
                name: "f_k_agronomist_invitations_cooperatives_cooperative_id",
                table: "agronomist_invitations",
                column: "cooperative_id",
                principalTable: "cooperatives",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_payments_subscriptions_subscription_id",
                table: "payments",
                column: "subscription_id",
                principalTable: "subscriptions",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_subscriptions__plan_plan_code",
                table: "subscriptions",
                column: "plan_code",
                principalTable: "plans",
                principalColumn: "code",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_agronomist_invitations_cooperatives_cooperative_id",
                table: "agronomist_invitations");

            migrationBuilder.DropForeignKey(
                name: "f_k_payments_subscriptions_subscription_id",
                table: "payments");

            migrationBuilder.DropForeignKey(
                name: "f_k_subscriptions__plan_plan_code",
                table: "subscriptions");

            migrationBuilder.AddForeignKey(
                name: "f_k_agronomist_invitations__cooperative_cooperative_id",
                table: "agronomist_invitations",
                column: "cooperative_id",
                principalTable: "cooperatives",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_payments__subscription_subscription_id",
                table: "payments",
                column: "subscription_id",
                principalTable: "subscriptions",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_subscriptions_plans_plan_code",
                table: "subscriptions",
                column: "plan_code",
                principalTable: "plans",
                principalColumn: "code",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
