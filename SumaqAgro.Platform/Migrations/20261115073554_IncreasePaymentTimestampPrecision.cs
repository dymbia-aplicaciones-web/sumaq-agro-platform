using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SumaqAgro.Platform.Migrations
{
    /// <inheritdoc />
    public partial class IncreasePaymentTimestampPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "attempted_at",
                table: "payments",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "attempted_at",
                table: "payments",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");
        }
    }
}
