using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class SettingsWaitingListCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "id", "description", "name", "type_of_settings_value", "value" },
                values: new object[] { new Guid("55c1ce71-5e65-44d0-b883-ab64c20c1517"), null, "Кол-во билетов на экране", 2, "10" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "password_hash",
                value: "$2a$11$Jd0inSInLeHPGyMTLkF1ROpyeb6dNjhgBoQ1byWnSQSdbYm0pNE52");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "password_hash",
                value: "$2a$11$M7oan6rhhUb.E.7syqkQgeu.KJYWBWQeD9iWizsbcbX1CpWorQ1X6");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "id",
                keyValue: new Guid("55c1ce71-5e65-44d0-b883-ab64c20c1517"));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "password_hash",
                value: "$2a$11$zCkxC.Po3uZOGDOFfC4Qw.Yd6i4y2G6CpcguDFnG7foYe.E3EcOdG");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "password_hash",
                value: "$2a$11$fSgPDOAMuNa4QtM5WDxC7OcAU7cV2R.AKUaPDGyetQS7QcZ5xFh3G");
        }
    }
}
