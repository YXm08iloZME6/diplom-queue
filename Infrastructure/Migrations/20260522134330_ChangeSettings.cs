using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class ChangeSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_settings_services_simple_mode_service_id",
                table: "settings");

            migrationBuilder.DropIndex(
                name: "ix_settings_simple_mode_service_id",
                table: "settings");

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "id", "simple_mode_service_id" },
                values: new object[] { new Guid("a55d9913-f36a-43a4-8321-272d22f85a2a"), new Guid("ef30bd6a-f192-4b25-8885-f7d679c6b313") });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "password_hash",
                value: "$2a$11$/Jva2uGhVJUa21FnqkekYecmtJW/OHNBeCQtm0UzOPK0Hsa9n5NUy");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "password_hash",
                value: "$2a$11$t9.Xr6GxDSTMparOA5N6UOlKdlw2eF.1rQQ4tuM0bnZ.VaCAcij/G");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "id",
                keyValue: new Guid("a55d9913-f36a-43a4-8321-272d22f85a2a"));

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "id", "simple_mode", "simple_mode_service_id" },
                values: new object[] { new Guid("44444444-4444-4444-4444-444444444444"), false, null });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "password_hash",
                value: "$2a$11$dPi99iqmF.nq.CeG.YKaFOa6KgCLyJjcFQsPB1X9GOBcEg2pFo9S6");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "password_hash",
                value: "$2a$11$KTfn0x0V0FYoOih6wu1LveO2WBvPLQhK5eEuS9g3wi2kO96/1B3E2");

            migrationBuilder.CreateIndex(
                name: "ix_settings_simple_mode_service_id",
                table: "settings",
                column: "simple_mode_service_id");

            migrationBuilder.AddForeignKey(
                name: "fk_settings_services_simple_mode_service_id",
                table: "settings",
                column: "simple_mode_service_id",
                principalTable: "services",
                principalColumn: "id");
        }
    }
}
