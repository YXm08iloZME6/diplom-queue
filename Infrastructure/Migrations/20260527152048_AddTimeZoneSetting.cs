using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddTimeZoneSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "id", "description", "name", "type_of_settings_value", "value" },
                values: new object[] { new Guid("c7dfb95f-9192-48c6-8c40-a92050dc4f4e"), null, "Часовой пояс", 2, "+4" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "password_hash",
                value: "$2a$11$beeOi21r2/b.OgSWybbG9.biwGC5WSbqgvnbyIxDnRHcpQ/QVFHN6");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "password_hash",
                value: "$2a$11$dXueE.gdo8Li6xAVW.YliOLaVja38rRTcKbOpjmKOoRKIW.KxP.bW");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "id",
                keyValue: new Guid("c7dfb95f-9192-48c6-8c40-a92050dc4f4e"));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "password_hash",
                value: "$2a$11$16O/zr/ECtdbSviwgnOaE.BHaazvr0NgxpYTOKfRjey15SEsvmuOa");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "password_hash",
                value: "$2a$11$euLqcNsX1OmJLmdasTs05OZiHhOpMMC.ble8hfovhT/gUn2ZcqkE2");
        }
    }
}
