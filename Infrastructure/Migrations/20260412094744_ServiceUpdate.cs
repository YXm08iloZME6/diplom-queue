using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class ServiceUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("9d78a673-efa3-4af3-9828-55515d26e134"),
                column: "icon_name",
                value: "Clock");

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("d320728d-0a5e-490c-be3c-04bcf3a7a4c8"),
                columns: new[] { "description", "icon_name" },
                values: new object[] { "Официальное подтверждение временной нетрудоспособности.", "CheckBook" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("9d78a673-efa3-4af3-9828-55515d26e134"),
                column: "icon_name",
                value: null);

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("d320728d-0a5e-490c-be3c-04bcf3a7a4c8"),
                columns: new[] { "description", "icon_name" },
                values: new object[] { "Официальное подтверждение временной нетрудоспособности с внесением данных в электронный реестр для передачи работодателю или в социальные службы.", null });
        }
    }
}
