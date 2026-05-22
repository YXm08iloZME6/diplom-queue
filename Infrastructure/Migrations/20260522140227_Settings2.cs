using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Settings2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("ef30bd6a-f192-4b25-8885-f7d679c6b313"),
                columns: new[] { "description", "icon_name", "name" },
                values: new object[] { "", "", "Простой мод" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("ef30bd6a-f192-4b25-8885-f7d679c6b313"),
                columns: new[] { "description", "icon_name", "name" },
                values: new object[] { "Мне просто спросить", "Lab", "Просто спросить" });

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
    }
}
