using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Queue.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    parentid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.id);
                    table.ForeignKey(
                        name: "FK_category_category_parentid",
                        column: x => x.parentid,
                        principalTable: "category",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    info = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "statuses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "services",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    letter = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    categoryid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_services", x => x.id);
                    table.ForeignKey(
                        name: "FK_services_category_categoryid",
                        column: x => x.categoryid,
                        principalTable: "category",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    surname = table.Column<string>(type: "text", nullable: true),
                    middlename = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    serviceid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_services_serviceid",
                        column: x => x.serviceid,
                        principalTable: "services",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "window",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    userid = table.Column<Guid>(type: "uuid", nullable: true),
                    serviceid = table.Column<Guid>(type: "uuid", nullable: true),
                    statusid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_window", x => x.id);
                    table.ForeignKey(
                        name: "FK_window_services_serviceid",
                        column: x => x.serviceid,
                        principalTable: "services",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_window_statuses_statusid",
                        column: x => x.statusid,
                        principalTable: "statuses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_window_users_userid",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    clientid = table.Column<Guid>(type: "uuid", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    number = table.Column<string>(type: "text", nullable: true),
                    estimationtime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    facets = table.Column<string>(type: "jsonb", nullable: false),
                    serviceid = table.Column<Guid>(type: "uuid", nullable: true),
                    statusid = table.Column<Guid>(type: "uuid", nullable: true),
                    windowid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tickets", x => x.id);
                    table.ForeignKey(
                        name: "FK_tickets_clients_clientid",
                        column: x => x.clientid,
                        principalTable: "clients",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tickets_services_serviceid",
                        column: x => x.serviceid,
                        principalTable: "services",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tickets_statuses_statusid",
                        column: x => x.statusid,
                        principalTable: "statuses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tickets_window_windowid",
                        column: x => x.windowid,
                        principalTable: "window",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_category_parentid",
                table: "category",
                column: "parentid");

            migrationBuilder.CreateIndex(
                name: "IX_services_categoryid",
                table: "services",
                column: "categoryid");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_clientid",
                table: "tickets",
                column: "clientid");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_serviceid",
                table: "tickets",
                column: "serviceid");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_statusid",
                table: "tickets",
                column: "statusid");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_windowid",
                table: "tickets",
                column: "windowid");

            migrationBuilder.CreateIndex(
                name: "IX_users_serviceid",
                table: "users",
                column: "serviceid");

            migrationBuilder.CreateIndex(
                name: "IX_window_serviceid",
                table: "window",
                column: "serviceid");

            migrationBuilder.CreateIndex(
                name: "IX_window_statusid",
                table: "window",
                column: "statusid");

            migrationBuilder.CreateIndex(
                name: "IX_window_userid",
                table: "window",
                column: "userid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tickets");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropTable(
                name: "window");

            migrationBuilder.DropTable(
                name: "statuses");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "services");

            migrationBuilder.DropTable(
                name: "category");
        }
    }
}
