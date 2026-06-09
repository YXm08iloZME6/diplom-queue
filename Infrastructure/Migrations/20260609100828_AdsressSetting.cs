using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AdsressSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "id", "description", "name", "type_of_settings_value", "value" },
                values: new object[] { new Guid("bad4163d-f386-4d46-a504-79f831d0301e"), null, "Адрес", 2, "Димитровград · Гоголя 21" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "password_hash",
                value: "$2a$11$JqV8T4pY73s1lqYEVY8eruNXK4HA7ZNF0VtCNyf/qyondsOVRMsUi");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "password_hash",
                value: "$2a$11$hzBcH6XwCoku1yISeoHkR.XWptyRADQIytI59OKjPAHHwsjg1rMqO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "id",
                keyValue: new Guid("bad4163d-f386-4d46-a504-79f831d0301e"));

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
    }
}
