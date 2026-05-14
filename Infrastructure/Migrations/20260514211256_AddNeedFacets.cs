using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddNeedFacets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_need_facets",
                table: "services",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("7370aa38-cbb9-4220-915d-ce042194f24e"),
                column: "is_need_facets",
                value: true);

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("99c48a22-122d-4821-afea-2b2b345e592c"),
                column: "is_need_facets",
                value: true);

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("9d78a673-efa3-4af3-9828-55515d26e134"),
                column: "is_need_facets",
                value: true);

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("d320728d-0a5e-490c-be3c-04bcf3a7a4c8"),
                column: "is_need_facets",
                value: true);

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc"),
                column: "is_need_facets",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_need_facets",
                table: "services");
        }
    }
}
