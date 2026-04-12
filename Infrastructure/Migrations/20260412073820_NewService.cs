using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class NewService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("7370aa38-cbb9-4260-915d-ce042194f24e"),
                column: "icon_name",
                value: "Lab");

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc"),
                column: "icon_name",
                value: "Book");

            migrationBuilder.InsertData(
                table: "services",
                columns: new[] { "id", "description", "icon_name", "letter", "name" },
                values: new object[] { new Guid("7370aa38-cbb9-4220-915d-ce042194f24e"), "Лабораторная диагностика от общих анализов крови до генетических исследований.", "Lab", "C", "Анализы" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("7370aa38-cbb9-4220-915d-ce042194f24e"));

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("7370aa38-cbb9-4260-915d-ce042194f24e"),
                column: "icon_name",
                value: "Book");

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc"),
                column: "icon_name",
                value: "Lab");
        }
    }
}
