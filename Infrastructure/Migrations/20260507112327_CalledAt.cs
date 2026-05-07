using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class CalledAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "operator_id",
                table: "windows");

            migrationBuilder.AddColumn<DateTime>(
                name: "called_at",
                table: "tickets",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "called_at",
                table: "tickets");

            migrationBuilder.AddColumn<Guid>(
                name: "operator_id",
                table: "windows",
                type: "uuid",
                nullable: true);
        }
    }
}
