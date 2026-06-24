using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class NewSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("ef30bd6a-f192-4b25-8885-f7d679c6b313"));

            // migrationBuilder.DeleteData(
            //     table: "settings",
            //     keyColumn: "id",
            //     keyValue: new Guid("a55d9913-f36a-43a4-8321-272d22f85a2a"));

            // migrationBuilder.DropColumn(
            //     name: "simple_mode",
            //     table: "settings");

            // migrationBuilder.DropColumn(
            //     name: "simple_mode_service_id",
            //     table: "settings");

            // migrationBuilder.AlterColumn<Guid>(
            //     name: "id",
            //     table: "settings", ...);

            // migrationBuilder.AddColumn<string>(
            //     name: "name",
            //     table: "settings", ...);

            // migrationBuilder.AddColumn<string>(
            //     name: "value",
            //     table: "settings", ...);

            // migrationBuilder.InsertData(
            //     table: "settings", ...);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "password_hash",
                value: "$2a$11$QAzWzsZHuqanj15vg5iGcuU/eF1TPdkX4k3Z1LwnNudFCySKl9R7u");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "password_hash",
                value: "$2a$11$vtKvi1936qU8J81D40e31.SUgdI6BMdvxM3zZnmOJOL4mUBnjFzuy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "id",
                keyValue: new Guid("aaca4ae5-734e-48ad-a34a-5b12f6c64212"));

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "id",
                keyValue: new Guid("b442d4a8-6b6d-42c6-b769-8c3dd4eb5147"));

            migrationBuilder.DropColumn(
                name: "name",
                table: "settings");

            migrationBuilder.DropColumn(
                name: "value",
                table: "settings");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                table: "settings",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AddColumn<bool>(
                name: "simple_mode",
                table: "settings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "simple_mode_service_id",
                table: "settings",
                type: "uuid",
                nullable: true);

            migrationBuilder.InsertData(
                table: "services",
                columns: new[] { "id", "description", "icon_name", "letter", "name", "parent_id" },
                values: new object[] { new Guid("ef30bd6a-f192-4b25-8885-f7d679c6b313"), "", "", "D", "Простой мод", null });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "id", "simple_mode", "simple_mode_service_id" },
                values: new object[] { new Guid("a55d9913-f36a-43a4-8321-272d22f85a2a"), false, new Guid("ef30bd6a-f192-4b25-8885-f7d679c6b313") });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "password_hash",
                value: "$2a$11$9SiLmIiX7ERk14tIpP/8I.MqcjsyVdzZ8yEUv/./q1jYjOAPn7PfG");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "password_hash",
                value: "$2a$11$3iKW5cyyV4Dtfyj/5i2jdOIzygFseSV5b9IyiljDZt5oSAGrsblIO");
        }
    }
}
