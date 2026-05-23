using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class NewSettings3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "value",
                table: "settings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "settings",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "id",
                keyValue: new Guid("aaca4ae5-734e-48ad-a34a-5b12f6c64212"),
                column: "name",
                value: "Буква для простого режима");

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "id",
                keyValue: new Guid("b442d4a8-6b6d-42c6-b769-8c3dd4eb5147"),
                column: "name",
                value: "Простой режим");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "settings");

            migrationBuilder.AlterColumn<string>(
                name: "value",
                table: "settings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "id",
                keyValue: new Guid("aaca4ae5-734e-48ad-a34a-5b12f6c64212"),
                column: "name",
                value: "Буква для простого мода");

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "id",
                keyValue: new Guid("b442d4a8-6b6d-42c6-b769-8c3dd4eb5147"),
                column: "name",
                value: "Простой мод");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "password_hash",
                value: "$2a$11$oh.fXzSmbxMETXV3c3VqXOYs3K9kShd7URJ2xhD7Z/K0.3IAoTLlO");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "password_hash",
                value: "$2a$11$gNpEs.krsjAaEw.fv008tepROrjE84TF27nyLPsb5DmcxXascWvVu");
        }
    }
}
