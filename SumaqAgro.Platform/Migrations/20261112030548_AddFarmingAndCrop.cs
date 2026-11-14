using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace SumaqAgro.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddFarmingAndCrop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "agroclimatic_alerts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "longtext", nullable: false),
                    description = table.Column<string>(type: "longtext", nullable: false),
                    severity = table.Column<string>(type: "longtext", nullable: false),
                    region = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    issued_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    source = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_agroclimatic_alerts", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "climate_forecasts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    region = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    temperature_celsius = table.Column<double>(type: "double", nullable: false),
                    rain_chance = table.Column<double>(type: "double", nullable: false),
                    forecast_date = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    source = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_climate_forecasts", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "field_inspections",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    pest_report_id = table.Column<long>(type: "bigint", nullable: false),
                    plot_id = table.Column<long>(type: "bigint", nullable: false),
                    agronomist_id = table.Column<long>(type: "bigint", nullable: false),
                    scheduled_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    status = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    findings = table.Column<string>(type: "longtext", nullable: true),
                    completed_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_field_inspections", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "field_plots",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    owner_user_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    region = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    status = table.Column<string>(type: "longtext", nullable: false),
                    area_hectares = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    agro_monitoring_polygon_id = table.Column<string>(type: "longtext", nullable: true),
                    boundary = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_field_plots", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pest_reports",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    plot_id = table.Column<long>(type: "bigint", nullable: false),
                    reporter_id = table.Column<long>(type: "bigint", nullable: false),
                    description = table.Column<string>(type: "longtext", nullable: false),
                    severity = table.Column<string>(type: "longtext", nullable: false),
                    status = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_pest_reports", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "regional_bulletins",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    agronomist_id = table.Column<long>(type: "bigint", nullable: false),
                    region = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    title = table.Column<string>(type: "longtext", nullable: false),
                    content = table.Column<string>(type: "longtext", nullable: false),
                    published_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_regional_bulletins", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "satellite_observations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    plot_id = table.Column<long>(type: "bigint", nullable: false),
                    captured_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    source = table.Column<string>(type: "longtext", nullable: false),
                    ndvi_mean = table.Column<double>(type: "double", nullable: true),
                    ndwi_mean = table.Column<double>(type: "double", nullable: true),
                    cloud_coverage_percent = table.Column<double>(type: "double", nullable: false),
                    surface_temp_kelvin = table.Column<double>(type: "double", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_satellite_observations", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "technical_prescriptions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    pest_report_id = table.Column<long>(type: "bigint", nullable: false),
                    plot_id = table.Column<long>(type: "bigint", nullable: false),
                    agronomist_id = table.Column<long>(type: "bigint", nullable: false),
                    product = table.Column<string>(type: "longtext", nullable: false),
                    dosage = table.Column<string>(type: "longtext", nullable: false),
                    instructions = table.Column<string>(type: "longtext", nullable: false),
                    application_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    applied_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_technical_prescriptions", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "crop_campaigns",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    field_plot_id = table.Column<long>(type: "bigint", nullable: false),
                    season = table.Column<string>(type: "longtext", nullable: false),
                    crop_type = table.Column<string>(type: "longtext", nullable: false),
                    seed_variety = table.Column<string>(type: "longtext", nullable: false),
                    custom_seed_variety = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    sowing_date = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    active_field_plot_id = table.Column<long>(type: "bigint", nullable: true, computedColumnSql: "CASE WHEN status = 'IN_PROGRESS' THEN field_plot_id ELSE NULL END", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_crop_campaigns", x => x.id);
                    table.ForeignKey(
                        name: "f_k_crop_campaigns__field_plot_field_plot_id",
                        column: x => x.field_plot_id,
                        principalTable: "field_plots",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_agroclimatic_alerts_region_issued_at",
                table: "agroclimatic_alerts",
                columns: new[] { "region", "issued_at" });

            migrationBuilder.CreateIndex(
                name: "i_x_climate_forecasts_region_forecast_date",
                table: "climate_forecasts",
                columns: new[] { "region", "forecast_date" });

            migrationBuilder.CreateIndex(
                name: "i_x_crop_campaigns_active_field_plot_id",
                table: "crop_campaigns",
                column: "active_field_plot_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_crop_campaigns_field_plot_id",
                table: "crop_campaigns",
                column: "field_plot_id");

            migrationBuilder.CreateIndex(
                name: "i_x_field_inspections_pest_report_id",
                table: "field_inspections",
                column: "pest_report_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_field_inspections_plot_id",
                table: "field_inspections",
                column: "plot_id");

            migrationBuilder.CreateIndex(
                name: "i_x_field_plots_owner_user_id_name",
                table: "field_plots",
                columns: new[] { "owner_user_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_pest_reports_plot_id",
                table: "pest_reports",
                column: "plot_id");

            migrationBuilder.CreateIndex(
                name: "i_x_regional_bulletins_region_published_at",
                table: "regional_bulletins",
                columns: new[] { "region", "published_at" });

            migrationBuilder.CreateIndex(
                name: "i_x_satellite_observations_plot_id_captured_at",
                table: "satellite_observations",
                columns: new[] { "plot_id", "captured_at" });

            migrationBuilder.CreateIndex(
                name: "i_x_technical_prescriptions_pest_report_id",
                table: "technical_prescriptions",
                column: "pest_report_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_technical_prescriptions_plot_id",
                table: "technical_prescriptions",
                column: "plot_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "agroclimatic_alerts");

            migrationBuilder.DropTable(
                name: "climate_forecasts");

            migrationBuilder.DropTable(
                name: "crop_campaigns");

            migrationBuilder.DropTable(
                name: "field_inspections");

            migrationBuilder.DropTable(
                name: "pest_reports");

            migrationBuilder.DropTable(
                name: "regional_bulletins");

            migrationBuilder.DropTable(
                name: "satellite_observations");

            migrationBuilder.DropTable(
                name: "technical_prescriptions");

            migrationBuilder.DropTable(
                name: "field_plots");
        }
    }
}
