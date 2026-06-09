using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class LogoSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "id", "description", "name", "type_of_settings_value", "value" },
                values: new object[] { new Guid("d4e5133b-edfa-4438-9945-76c383dd2a33"), null, "Логотип", 2, "" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "password_hash",
                value: "$2a$11$nPaZoE.2wOxxUuxNHLcjyeJZNj59dmynLYIsRdRFofb7BMvbXMWiC");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "password_hash",
                value: "$2a$11$xUPdfbfQR.YNAGhTqv6y7.c4F3k.obdpAAOYkOxJi3h8Wux.gGumi");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "id",
                keyValue: new Guid("d4e5133b-edfa-4438-9945-76c383dd2a33"));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "password_hash",
                value: "$2a$11$vJRXW4KkMivemLF8RRfFX.kTq2Ms0Z4Aw8Ba8rGKJCuxyEd1.CZ1K");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "password_hash",
                value: "$2a$11$KN.usI/Mu3834O9BYf7k3ezgaOq4CnLUjfLaIYi1IUe5vxUf1agJ.");
        }
    }
}
