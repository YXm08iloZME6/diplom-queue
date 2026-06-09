using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class ServiceImageSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "image_path",
                table: "services",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "id", "description", "name", "type_of_settings_value", "value" },
                values: new object[] { new Guid("27291322-9012-4c02-8869-9347f5dac6da"), null, "Картинки у сервисов", 0, "false" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "password_hash",
                value: "$2a$11$witt/j.y1GTSFluypaauS.Lx8LpA05Ttq5l7fkBlaiZdXn1WYHm..");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "password_hash",
                value: "$2a$11$v/6MB/MrbcmrEE3v4dPmzuauEOhu3J2WH6i82CNt3dZimEVXxXpsS");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "id",
                keyValue: new Guid("27291322-9012-4c02-8869-9347f5dac6da"));

            migrationBuilder.DropColumn(
                name: "image_path",
                table: "services");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "password_hash",
                value: "$2a$11$JqV8T4pY73s1lqYEVY8eruNXK4HA7ZNF0VtCNyf/qyondsOVRMsUi");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "password_hash",
                value: "$2a$11$hzBcH6XwCoku1yISeoHkR.XWptyRADQIytI59OKjPAHHwsjg1rMqO");
        }
    }
}
