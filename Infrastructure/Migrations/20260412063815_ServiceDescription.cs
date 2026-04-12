using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class ServiceDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "services",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("7370aa38-cbb9-4260-915d-ce042194f24e"),
                column: "description",
                value: "Лабораторная диагностика от общих анализов крови до генетических исследований.");

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("99c48a22-122d-4821-afea-2b2b345e592c"),
                column: "description",
                value: "Оформление и оплата медицинских услуг, не входящих в программу ОМС.");

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("9d78a673-efa3-4af3-9828-55515d26e134"),
                column: "description",
                value: "Выбор специалиста и бронирование подходящего времени визита.");

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("d320728d-0a5e-490c-be3c-04bcf3a7a4c8"),
                column: "description",
                value: "Официальное подтверждение временной нетрудоспособности с внесением данных в электронный реестр для передачи работодателю или в социальные службы.");

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc"),
                column: "description",
                value: "Запись на первичный прием, заведение медицинских карт и предоставление справочной информации о работе клиники.");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "services");
        }
    }
}
