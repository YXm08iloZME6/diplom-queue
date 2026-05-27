using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class settings3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "password_hash",
                value: "$2a$11$mRztWmw7kyjZzT1c1xw9VOwOs897R1EDuByS4WG6Qkw8wga3GpRhS");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "password_hash",
                value: "$2a$11$MsiFcOfP.SPXS8xAHu8v2OG9BBGFWSKQXQ7NIF41v7PazYuubi.Nq");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "password_hash",
                value: "$2a$11$16O/zr/ECtdbSviwgnOaE.BHaazvr0NgxpYTOKfRjey15SEsvmuOa");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "password_hash",
                value: "$2a$11$1xlhnDiEh.C6fr1aJz8ipe6gbB4UVqsGsVjsnlS6UuvffE1OG36/y");
        }
    }
}
