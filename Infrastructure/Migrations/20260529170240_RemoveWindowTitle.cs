using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class RemoveWindowTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "title",
                table: "windows");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "password_hash",
                value: "$2a$11$1cNf3V14wcbKiL25kt.KzuWus1JPTmMPJsF6ox7DgZ21NGxa5.1te");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "password_hash",
                value: "$2a$11$SKitD5pcFPSRg3r5st/FruIo5WWQEII7GdGK1jGN0nvdSbWFoIy/a");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "windows",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "password_hash",
                value: "$2a$11$7TnDM4XHRML300CIyfP1p.vpjzTQ.aDiWtN0/Ff0hnVHtJRPsT5Wi");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "password_hash",
                value: "$2a$11$2dH1C40o84BlOXX1oYS/Du3f4aTMccOzURYtKg.cxD7IsHCPy.y0S");

            migrationBuilder.InsertData(
                table: "windows",
                columns: new[] { "id", "number", "service_id", "status", "title" },
                values: new object[,]
                {
                    { new Guid("2b040596-6b16-492b-922b-3a2662594b6f"), "1", new Guid("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc"), "Open", "Регистратура" },
                    { new Guid("76033f8f-0688-4ee8-805a-b1dac8c6f469"), "3", new Guid("7370aa38-cbb9-4220-915d-ce042194f24e"), "Open", "Регистратура" },
                    { new Guid("d26ebc52-2b3a-42e7-95c1-b927ff1d1e4a"), "2", new Guid("99c48a22-122d-4821-afea-2b2b345e592c"), "Open", "Регистратура" }
                });
        }
    }
}
