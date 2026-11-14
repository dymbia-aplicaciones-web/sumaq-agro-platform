using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace SumaqAgro.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "agronomist_assignments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    cooperative_id = table.Column<long>(type: "bigint", nullable: false),
                    agronomist_user_id = table.Column<long>(type: "bigint", nullable: false),
                    plot_id = table.Column<long>(type: "bigint", nullable: false),
                    assigned_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    active_plot_id = table.Column<long>(type: "bigint", nullable: true, computedColumnSql: "CASE WHEN active THEN plot_id ELSE NULL END", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_agronomist_assignments", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cooperatives",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ruc = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false),
                    business_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    region = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    manager_user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_cooperatives", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "profiles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    first_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    phone_number = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true),
                    photo_url = table.Column<string>(type: "longtext", nullable: true),
                    preferred_language = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_profiles", x => x.id);
                    table.ForeignKey(
                        name: "f_k_profiles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "agronomist_invitations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    cooperative_id = table.Column<long>(type: "bigint", nullable: false),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    status = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    sent_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_agronomist_invitations", x => x.id);
                    table.ForeignKey(
                        name: "f_k_agronomist_invitations__cooperative_cooperative_id",
                        column: x => x.cooperative_id,
                        principalTable: "cooperatives",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cooperative_members",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    cooperative_id = table.Column<long>(type: "bigint", nullable: false),
                    dni = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false),
                    first_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    community = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    joined_at = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_cooperative_members", x => x.id);
                    table.ForeignKey(
                        name: "f_k_cooperative_members_cooperatives_cooperative_id",
                        column: x => x.cooperative_id,
                        principalTable: "cooperatives",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_agronomist_assignments_active_plot_id",
                table: "agronomist_assignments",
                column: "active_plot_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_agronomist_assignments_cooperative_id",
                table: "agronomist_assignments",
                column: "cooperative_id");

            migrationBuilder.CreateIndex(
                name: "i_x_agronomist_invitations_cooperative_id",
                table: "agronomist_invitations",
                column: "cooperative_id");

            migrationBuilder.CreateIndex(
                name: "i_x_cooperative_members_cooperative_id_dni",
                table: "cooperative_members",
                columns: new[] { "cooperative_id", "dni" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_cooperatives_ruc",
                table: "cooperatives",
                column: "ruc",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_profiles_user_id",
                table: "profiles",
                column: "user_id",
                unique: true);

            migrationBuilder.Sql("""
                INSERT INTO profiles (user_id, first_name, last_name, preferred_language)
                SELECT id, first_name, last_name, 'ES' FROM users
                """);
            migrationBuilder.DropColumn(
                name: "first_name",
                table: "users");

            migrationBuilder.DropColumn(
                name: "last_name",
                table: "users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "agronomist_assignments");

            migrationBuilder.DropTable(
                name: "agronomist_invitations");

            migrationBuilder.DropTable(
                name: "cooperative_members");



            migrationBuilder.DropTable(
                name: "cooperatives");

            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "users",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                table: "users",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("""
                UPDATE users
                INNER JOIN profiles ON profiles.user_id = users.id
                SET users.first_name = profiles.first_name,
                    users.last_name = profiles.last_name
                """);

            migrationBuilder.DropTable(name: "profiles");
        }
    }
}
