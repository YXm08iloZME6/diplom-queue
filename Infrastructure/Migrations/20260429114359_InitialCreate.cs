using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "services",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    letter = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    icon_name = table.Column<string>(type: "text", nullable: true),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_services", x => x.id);
                    table.ForeignKey(
                        name: "fk_services_services_parent_id",
                        column: x => x.parent_id,
                        principalTable: "services",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    estimation_time = table.Column<TimeSpan>(type: "interval", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    number = table.Column<string>(type: "text", nullable: false),
                    facets = table.Column<string>(type: "jsonb", nullable: true),
                    client_id = table.Column<Guid>(type: "uuid", nullable: true),
                    service_id = table.Column<Guid>(type: "uuid", nullable: true),
                    window_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tickets", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "services",
                columns: new[] { "id", "description", "icon_name", "letter", "name", "parent_id" },
                values: new object[,]
                {
                    { new Guid("7370aa38-cbb9-4220-915d-ce042194f24e"), "Лабораторная диагностика от общих анализов крови до генетических исследований.", "Lab", "C", "Анализы", null },
                    { new Guid("99c48a22-122d-4821-afea-2b2b345e592c"), "Оформление и оплата медицинских услуг, не входящих в программу ОМС.", "Ruble", "B", "Платные услуги", null },
                    { new Guid("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc"), "Запись на первичный прием, заведение медицинских карт и предоставление справочной информации о работе клиники.", "Book", "A", "Регистратура", null }
                });

            migrationBuilder.InsertData(
                table: "services",
                columns: new[] { "id", "description", "icon_name", "letter", "name", "parent_id" },
                values: new object[,]
                {
                    { new Guid("9d78a673-efa3-4af3-9828-55515d26e134"), "Выбор специалиста и бронирование подходящего времени визита.", "Clock", null, "Запись на прием к врачу", new Guid("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc") },
                    { new Guid("d320728d-0a5e-490c-be3c-04bcf3a7a4c8"), "Официальное подтверждение временной нетрудоспособности.", "CheckBook", null, "Оформление больничного", new Guid("dfc3d5c0-69fc-4ac1-a593-473b945dd3bc") }
                });

            migrationBuilder.CreateIndex(
                name: "ix_services_parent_id",
                table: "services",
                column: "parent_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "services");

            migrationBuilder.DropTable(
                name: "tickets");
        }
    }
}
