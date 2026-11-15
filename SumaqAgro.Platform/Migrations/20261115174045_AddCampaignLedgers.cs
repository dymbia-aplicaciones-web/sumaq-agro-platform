using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SumaqAgro.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddCampaignLedgers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "campaign_ledgers",
                columns: table => new
                {
                    campaign_id = table.Column<long>(type: "bigint", nullable: false),
                    payload = table.Column<string>(type: "json", nullable: false),
                    version = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_campaign_ledgers", x => x.campaign_id);
                    table.ForeignKey(
                        name: "f_k_campaign_ledgers__crop_campaign_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "crop_campaigns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "campaign_ledgers");
        }
    }
}
