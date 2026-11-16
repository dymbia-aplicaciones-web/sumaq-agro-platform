using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SumaqAgro.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldPlotSoilBaseline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "organic_matter_percentage",
                table: "field_plots",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ph",
                table: "field_plots",
                type: "decimal(4,2)",
                precision: 4,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "recorded_at",
                table: "field_plots",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "texture",
                table: "field_plots",
                type: "varchar(16)",
                maxLength: 16,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "organic_matter_percentage",
                table: "field_plots");

            migrationBuilder.DropColumn(
                name: "ph",
                table: "field_plots");

            migrationBuilder.DropColumn(
                name: "recorded_at",
                table: "field_plots");

            migrationBuilder.DropColumn(
                name: "texture",
                table: "field_plots");
        }
    }
}
