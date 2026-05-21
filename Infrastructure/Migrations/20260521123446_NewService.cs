using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class NewService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "services",
                columns: new[] { "id", "description", "icon_name", "letter", "name", "need_more_info", "parent_id" },
                values: new object[] { new Guid("ef30bd6a-f192-4b25-8885-f7d679c6b313"), "Мне просто спросить", "Lab", "D", "Просто спросить", true, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("ef30bd6a-f192-4b25-8885-f7d679c6b313"));
        }
    }
}
