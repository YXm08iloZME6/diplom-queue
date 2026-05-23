using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "need_more_info",
            //    table: "services");

            //migrationBuilder.AddColumn<bool>(
            //    name: "is_active",
            //    table: "services",
            //    type: "boolean",
            //    nullable: false,
            //    defaultValue: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "is_need_facets",
            //    table: "services",
            //    type: "boolean",
            //    nullable: false,
            //    defaultValue: true);

            migrationBuilder.CreateTable(
                name: "settings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    simple_mode = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    simple_mode_service_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_settings", x => x.id);
                });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("ef30bd6a-f192-4b25-8885-f7d679c6b313"),
                columns: new[] { "description", "icon_name", "name" },
                values: new object[] { "", "", "Простой мод" });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "id", "simple_mode_service_id" },
                values: new object[] { new Guid("a55d9913-f36a-43a4-8321-272d22f85a2a"), new Guid("ef30bd6a-f192-4b25-8885-f7d679c6b313") });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "middle_name", "name", "password_hash", "status", "surname", "window_id" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "admin@admin", null, null, "$2a$11$bCnXxt2KGEYyV3gdD6wFi.2oGmbQskuQ2SnMefX7sKPgS9xvSqCnG", 0, null, null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "operator@operator", null, null, "$2a$11$euLqcNsX1OmJLmdasTs05OZiHhOpMMC.ble8hfovhT/gUn2ZcqkE2", 0, null, null }
                });

            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "role_id", "user_id" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("22222222-2222-2222-2222-222222222222") }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "settings");

            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumns: new[] { "role_id", "user_id" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumns: new[] { "role_id", "user_id" },
                keyValues: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumns: new[] { "role_id", "user_id" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "services");

            migrationBuilder.DropColumn(
                name: "is_need_facets",
                table: "services");

            migrationBuilder.AddColumn<bool>(
                name: "need_more_info",
                table: "services",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("ef30bd6a-f192-4b25-8885-f7d679c6b313"),
                columns: new[] { "description", "icon_name", "name", "need_more_info" },
                values: new object[] { "Мне просто спросить", "Lab", "Просто спросить", true });
        }
    }
}
