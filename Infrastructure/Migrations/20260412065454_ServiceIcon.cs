using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class ServiceIcon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "icon_name",
                table: "services",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("7370aa38-cbb9-4260-915d-ce042194f24e"),
                column: "icon_name",
                value: "Book");

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("99c48a22-122d-4821-afea-2b2b345e592c"),
                column: "icon_name",
                value: "Ruble");

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc"),
                column: "icon_name",
                value: "Lab");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "icon_name",
                table: "services");
        }
    }
}
