using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JackSite.Authentication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "client_basics",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    secret = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    confidential = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    protocol = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "openid-connect"),
                    access_token_lifespan = table.Column<int>(type: "int", nullable: false),
                    refresh_token_lifespan = table.Column<int>(type: "int", nullable: false),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_basics", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_session",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    ip_address = table.Column<string>(type: "longtext", nullable: false),
                    user_agent = table.Column<string>(type: "longtext", nullable: true),
                    start_time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    last_access = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_session", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_session_user_basics_user_id",
                        column: x => x.user_id,
                        principalTable: "user_basics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "client_cors_origins",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    client_id = table.Column<long>(type: "bigint", nullable: false),
                    origin = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_cors_origins", x => x.id);
                    table.ForeignKey(
                        name: "fk_client_cors_origins_client_basics_client_id",
                        column: x => x.client_id,
                        principalTable: "client_basics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "client_redirect_uris",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    client_id = table.Column<long>(type: "bigint", nullable: false),
                    uri = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_redirect_uris", x => x.id);
                    table.ForeignKey(
                        name: "fk_client_redirect_uris_client_basics_client_id",
                        column: x => x.client_id,
                        principalTable: "client_basics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "client_scopes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    client_id = table.Column<long>(type: "bigint", nullable: false),
                    scope = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_scopes", x => x.id);
                    table.ForeignKey(
                        name: "fk_client_scopes_client_basics_client_id",
                        column: x => x.client_id,
                        principalTable: "client_basics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "client_sessions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    user_session_id = table.Column<long>(type: "bigint", nullable: false),
                    client_id = table.Column<long>(type: "bigint", nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserSessionId1 = table.Column<long>(type: "bigint", nullable: true),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_sessions", x => x.id);
                    table.ForeignKey(
                        name: "FK_client_sessions_user_session_UserSessionId1",
                        column: x => x.UserSessionId1,
                        principalTable: "user_session",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_client_sessions_client_basics_client_id",
                        column: x => x.client_id,
                        principalTable: "client_basics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_client_sessions_user_session_user_session_id",
                        column: x => x.user_session_id,
                        principalTable: "user_session",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ix_client_cors_origins_client_id",
                table: "client_cors_origins",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_redirect_uris_client_id",
                table: "client_redirect_uris",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_scopes_client_id",
                table: "client_scopes",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_sessions_client_id",
                table: "client_sessions",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_sessions_user_session_id",
                table: "client_sessions",
                column: "user_session_id");

            migrationBuilder.CreateIndex(
                name: "IX_client_sessions_UserSessionId1",
                table: "client_sessions",
                column: "UserSessionId1");

            migrationBuilder.CreateIndex(
                name: "ix_user_session_user_id",
                table: "user_session",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "client_cors_origins");

            migrationBuilder.DropTable(
                name: "client_redirect_uris");

            migrationBuilder.DropTable(
                name: "client_scopes");

            migrationBuilder.DropTable(
                name: "client_sessions");

            migrationBuilder.DropTable(
                name: "user_session");

            migrationBuilder.DropTable(
                name: "client_basics");
        }
    }
}
