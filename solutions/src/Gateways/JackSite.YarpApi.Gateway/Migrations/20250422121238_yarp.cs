using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JackSite.YarpApi.Gateway.Migrations
{
    /// <inheritdoc />
    public partial class yarp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "yarp_configs",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    config_json = table.Column<string>(type: "TEXT", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false),
                    last_modified = table.Column<DateTime>(type: "TEXT", nullable: false),
                    created_time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    create_at = table.Column<long>(type: "INTEGER", nullable: false),
                    updated_time = table.Column<DateTime>(type: "TEXT", nullable: true),
                    update_at = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_yarp_configs", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_yarp_configs_is_active",
                table: "yarp_configs",
                column: "is_active");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "yarp_configs");
        }
    }
}
