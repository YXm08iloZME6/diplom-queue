using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class ServiceUpdate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "parent_services");

            migrationBuilder.DeleteData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("7370aa38-cbb9-4260-915d-ce042194f24e"));

            migrationBuilder.AddColumn<Guid>(
                name: "parent_id",
                table: "services",
                type: "uuid",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("9d78a673-efa3-4af3-9828-55515d26e134"),
                column: "parent_id",
                value: new Guid("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc"));

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("d320728d-0a5e-490c-be3c-04bcf3a7a4c8"),
                column: "parent_id",
                value: new Guid("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc"));

            migrationBuilder.CreateIndex(
                name: "ix_services_parent_id",
                table: "services",
                column: "parent_id");

            migrationBuilder.AddForeignKey(
                name: "fk_services_services_parent_id",
                table: "services",
                column: "parent_id",
                principalTable: "services",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_services_services_parent_id",
                table: "services");

            migrationBuilder.DropIndex(
                name: "ix_services_parent_id",
                table: "services");

            migrationBuilder.DropColumn(
                name: "parent_id",
                table: "services");

            migrationBuilder.CreateTable(
                name: "parent_services",
                columns: table => new
                {
                    children_id = table.Column<Guid>(type: "uuid", nullable: false),
                    parents_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_parent_services", x => new { x.children_id, x.parents_id });
                    table.ForeignKey(
                        name: "fk_parent_services_services_children_id",
                        column: x => x.children_id,
                        principalTable: "services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_parent_services_services_parents_id",
                        column: x => x.parents_id,
                        principalTable: "services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "parent_services",
                columns: new[] { "children_id", "parents_id" },
                values: new object[,]
                {
                    { new Guid("9d78a673-efa3-4af3-9828-55515d26e134"), new Guid("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc") },
                    { new Guid("d320728d-0a5e-490c-be3c-04bcf3a7a4c8"), new Guid("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc") }
                });

            migrationBuilder.InsertData(
                table: "services",
                columns: new[] { "id", "description", "icon_name", "letter", "name" },
                values: new object[] { new Guid("7370aa38-cbb9-4260-915d-ce042194f24e"), "Лабораторная диагностика от общих анализов крови до генетических исследований.", "Lab", "C", "Анализы" });

            migrationBuilder.CreateIndex(
                name: "ix_parent_services_parents_id",
                table: "parent_services",
                column: "parents_id");
        }
    }
}
