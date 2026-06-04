using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class ColorSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "id", "description", "name", "type_of_settings_value", "value" },
                values: new object[] { new Guid("3b9c1f04-7a2e-4d61-8c55-9e0a1b2c3d4e"), null, "Основной цвет", 2, "#1e8f5e" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "id",
                keyValue: new Guid("3b9c1f04-7a2e-4d61-8c55-9e0a1b2c3d4e"));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "password_hash",
                value: "$2a$11$F17PJiOk/MoEzv9IvCK9bu3OVv0Q96p3.RTKcvOLSu1i7HAy6tPrm");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "password_hash",
                value: "$2a$11$e4LuEfEwsgbnicnt0Q5x0.fGsIu1i3CeL7fNl72RvMmOVv8QgfP8C");
        }
    }
}
