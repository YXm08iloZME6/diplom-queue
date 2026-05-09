using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddWindows : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_users_services_service_id",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_users_service_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "service_id",
                table: "users");

            migrationBuilder.AddColumn<Guid>(
                name: "window_id",
                table: "users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "tickets",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<DateTime>(
                name: "completed_at",
                table: "tickets",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "redirect_comment",
                table: "tickets",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "started_at",
                table: "tickets",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "windows",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    service_id = table.Column<Guid>(type: "uuid", nullable: false),
                    operator_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_windows", x => x.id);
                    table.ForeignKey(
                        name: "fk_windows_services_service_id",
                        column: x => x.service_id,
                        principalTable: "services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_users_window_id",
                table: "users",
                column: "window_id");

            migrationBuilder.CreateIndex(
                name: "ix_tickets_window_id",
                table: "tickets",
                column: "window_id");

            migrationBuilder.CreateIndex(
                name: "ix_windows_service_id",
                table: "windows",
                column: "service_id");

            migrationBuilder.AddForeignKey(
                name: "fk_tickets_windows_window_id",
                table: "tickets",
                column: "window_id",
                principalTable: "windows",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_users_windows_window_id",
                table: "users",
                column: "window_id",
                principalTable: "windows",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tickets_windows_window_id",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "fk_users_windows_window_id",
                table: "users");

            migrationBuilder.DropTable(
                name: "windows");

            migrationBuilder.DropIndex(
                name: "ix_users_window_id",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_tickets_window_id",
                table: "tickets");

            migrationBuilder.DropColumn(
                name: "window_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "completed_at",
                table: "tickets");

            migrationBuilder.DropColumn(
                name: "redirect_comment",
                table: "tickets");

            migrationBuilder.DropColumn(
                name: "started_at",
                table: "tickets");

            migrationBuilder.AddColumn<Guid>(
                name: "service_id",
                table: "users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "tickets",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_service_id",
                table: "users",
                column: "service_id");

            migrationBuilder.AddForeignKey(
                name: "fk_users_services_service_id",
                table: "users",
                column: "service_id",
                principalTable: "services",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
