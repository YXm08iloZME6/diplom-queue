using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class NameSettig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "id", "description", "name", "type_of_settings_value", "value" },
                values: new object[] { new Guid("dc944d51-d0d0-4005-8d84-5d0a2c6b2530"), null, "Название организации", 2, "Лекон" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "id",
                keyValue: new Guid("dc944d51-d0d0-4005-8d84-5d0a2c6b2530"));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "password_hash",
                value: "$2a$11$9xIx86z.LGf1qTmnNzAE8ugjc80Zwto/e9K5x52tZgsnI/udeg/D.");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "password_hash",
                value: "$2a$11$YY5qfkb/0uwgl9UkmT5YWeWKki28UEFGT9jjyydHbfSKlUkIXOFrW");
        }
    }
}
