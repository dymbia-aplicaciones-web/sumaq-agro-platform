using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SumaqAgro.Platform.Migrations
{
    /// <inheritdoc />
    public partial class LinkCooperativeMembersToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "user_id",
                table: "cooperative_members",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "i_x_cooperative_members_user_id",
                table: "cooperative_members",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "i_x_cooperative_members_user_id",
                table: "cooperative_members");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "cooperative_members");
        }
    }
}
