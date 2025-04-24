using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JackSite.YarpApi.Gateway.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "request_logs",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false),
                    path = table.Column<string>(type: "TEXT", nullable: false),
                    method = table.Column<string>(type: "TEXT", nullable: false),
                    query_string = table.Column<string>(type: "TEXT", nullable: false),
                    request_body = table.Column<string>(type: "TEXT", nullable: false),
                    request_headers = table.Column<string>(type: "TEXT", nullable: true),
                    response_headers = table.Column<string>(type: "TEXT", nullable: true),
                    response_body = table.Column<string>(type: "TEXT", nullable: true),
                    status_code = table.Column<int>(type: "INTEGER", nullable: false),
                    error_message = table.Column<string>(type: "TEXT", nullable: true),
                    stack_trace = table.Column<string>(type: "TEXT", nullable: true),
                    client_ip = table.Column<string>(type: "TEXT", nullable: true),
                    user_agent = table.Column<string>(type: "TEXT", nullable: true),
                    request_time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    response_time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    execution_time = table.Column<long>(type: "INTEGER", nullable: false),
                    target_service = table.Column<string>(type: "TEXT", nullable: false),
                    created_time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    create_at = table.Column<long>(type: "INTEGER", nullable: false),
                    updated_time = table.Column<DateTime>(type: "TEXT", nullable: true),
                    update_at = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_request_logs", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "request_logs");
        }
    }
}
