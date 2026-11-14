using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SumaqAgro.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddCertificationAndSubscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "harvest_batches",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    batch_code = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    cooperative_id = table.Column<long>(type: "bigint", nullable: false),
                    member_id = table.Column<long>(type: "bigint", nullable: false),
                    plot_id = table.Column<long>(type: "bigint", nullable: false),
                    crop_type = table.Column<string>(type: "longtext", nullable: false),
                    harvested_on = table.Column<DateTime>(type: "date", nullable: false),
                    registered_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    status = table.Column<string>(type: "longtext", nullable: false),
                    net_weight_kg = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: true),
                    sample_identifier = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true),
                    sample_weight_kg = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: true),
                    first_caliber_percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    second_caliber_percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    third_caliber_percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    pest_damage_percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    coffee_cupping_score = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    coffee_ferment_defect = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    quality_score = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    quality_category = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_harvest_batches", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "plans",
                columns: table => new
                {
                    code = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    monthly_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    annual_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    registered_plot_limit = table.Column<int>(type: "int", nullable: false),
                    producer_limit = table.Column<int>(type: "int", nullable: false),
                    supervised_plot_limit = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_plans", x => x.code);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "quality_certificates",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    certificate_number = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    harvest_batch_id = table.Column<long>(type: "bigint", nullable: false),
                    quality_category = table.Column<string>(type: "longtext", nullable: false),
                    certificate_hash = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    issued_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    issued_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    status = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_quality_certificates", x => x.id);
                    table.ForeignKey(
                        name: "f_k_quality_certificates_harvest_batches_harvest_batch_id",
                        column: x => x.harvest_batch_id,
                        principalTable: "harvest_batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "subscriptions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    plan_code = table.Column<string>(type: "varchar(32)", nullable: false),
                    billing_cycle = table.Column<string>(type: "longtext", nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: false),
                    status = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_subscriptions", x => x.id);
                    table.ForeignKey(
                        name: "f_k_subscriptions_plans_plan_code",
                        column: x => x.plan_code,
                        principalTable: "plans",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_subscriptions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    reference = table.Column<string>(type: "varchar(41)", maxLength: 41, nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    subscription_id = table.Column<long>(type: "bigint", nullable: true),
                    plan_code = table.Column<string>(type: "varchar(32)", nullable: false),
                    billing_cycle = table.Column<string>(type: "longtext", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    status = table.Column<string>(type: "longtext", nullable: false),
                    attempted_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_payments", x => x.reference);
                    table.ForeignKey(
                        name: "f_k_payments__plan_plan_code",
                        column: x => x.plan_code,
                        principalTable: "plans",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_payments__subscription_subscription_id",
                        column: x => x.subscription_id,
                        principalTable: "subscriptions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "f_k_payments_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "plans",
                columns: new[] { "code", "annual_price", "monthly_price", "producer_limit", "registered_plot_limit", "supervised_plot_limit" },
                values: new object[,]
                {
                    { "COOPERATIVE_PRO", 1890m, 189m, 50, 50, 0 },
                    { "SEED", 490m, 49m, 0, 4, 0 },
                    { "TECHNICAL_ADVISOR", 890m, 89m, 0, 0, 20 }
                });

            migrationBuilder.CreateIndex(
                name: "i_x_harvest_batches_batch_code",
                table: "harvest_batches",
                column: "batch_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_payments_plan_code",
                table: "payments",
                column: "plan_code");

            migrationBuilder.CreateIndex(
                name: "i_x_payments_subscription_id",
                table: "payments",
                column: "subscription_id");

            migrationBuilder.CreateIndex(
                name: "i_x_payments_user_id",
                table: "payments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "i_x_quality_certificates_certificate_number",
                table: "quality_certificates",
                column: "certificate_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_quality_certificates_harvest_batch_id",
                table: "quality_certificates",
                column: "harvest_batch_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_subscriptions_plan_code",
                table: "subscriptions",
                column: "plan_code");

            migrationBuilder.CreateIndex(
                name: "i_x_subscriptions_user_id",
                table: "subscriptions",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "quality_certificates");

            migrationBuilder.DropTable(
                name: "subscriptions");

            migrationBuilder.DropTable(
                name: "harvest_batches");

            migrationBuilder.DropTable(
                name: "plans");
        }
    }
}
