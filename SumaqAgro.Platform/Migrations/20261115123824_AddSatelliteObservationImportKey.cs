using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SumaqAgro.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddSatelliteObservationImportKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "import_key",
                table: "satellite_observations",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "i_x_satellite_observations_import_key",
                table: "satellite_observations",
                column: "import_key",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "i_x_satellite_observations_import_key",
                table: "satellite_observations");

            migrationBuilder.DropColumn(
                name: "import_key",
                table: "satellite_observations");
        }
    }
}
