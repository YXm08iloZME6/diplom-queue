using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class ServiceChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "letter",
                table: "services",
                type: "character varying(1)",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1);

            migrationBuilder.InsertData(
                table: "services",
                columns: new[] { "id", "letter", "name" },
                values: new object[,]
                {
                    { new Guid("7370aa38-cbb9-4260-915d-ce042194f24e"), "C", "Анализы" },
                    { new Guid("99c48a22-122d-4821-afea-2b2b345e592c"), "B", "Платные услуги" },
                    { new Guid("9d78a673-efa3-4af3-9828-55515d26e134"), null, "Запись на прием к врачу" },
                    { new Guid("d320728d-0a5e-490c-be3c-04bcf3a7a4c8"), null, "Оформление больничного" },
                    { new Guid("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc"), "A", "Регистратура" }
                });

            migrationBuilder.InsertData(
                table: "parent_services",
                columns: new[] { "children_id", "parents_id" },
                values: new object[,]
                {
                    { new Guid("9d78a673-efa3-4af3-9828-55515d26e134"), new Guid("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc") },
                    { new Guid("d320728d-0a5e-490c-be3c-04bcf3a7a4c8"), new Guid("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc") }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "parent_services",
                keyColumns: new[] { "children_id", "parents_id" },
                keyValues: new object[] { new Guid("9d78a673-efa3-4af3-9828-55515d26e134"), new Guid("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc") });

            migrationBuilder.DeleteData(
                table: "parent_services",
                keyColumns: new[] { "children_id", "parents_id" },
                keyValues: new object[] { new Guid("d320728d-0a5e-490c-be3c-04bcf3a7a4c8"), new Guid("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc") });

            migrationBuilder.DeleteData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("7370aa38-cbb9-4260-915d-ce042194f24e"));

            migrationBuilder.DeleteData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("99c48a22-122d-4821-afea-2b2b345e592c"));

            migrationBuilder.DeleteData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("9d78a673-efa3-4af3-9828-55515d26e134"));

            migrationBuilder.DeleteData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("d320728d-0a5e-490c-be3c-04bcf3a7a4c8"));

            migrationBuilder.DeleteData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc"));

            migrationBuilder.AlterColumn<string>(
                name: "letter",
                table: "services",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1,
                oldNullable: true);
        }
    }
}
