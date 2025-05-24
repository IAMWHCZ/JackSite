using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JackSite.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "logs",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    level = table.Column<short>(type: "smallint", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    source = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    message = table.Column<string>(type: "text", nullable: false),
                    exception = table.Column<string>(type: "text", nullable: true),
                    properties = table.Column<string>(type: "jsonb", nullable: true),
                    request_path = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    request_method = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    client_ip = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    machine_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    code = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    is_system = table.Column<bool>(type: "boolean", nullable: false),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    is_system = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    salt = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    phone_number = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    avatar_url = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    last_login_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_login_ip = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    login_count = table.Column<int>(type: "integer", nullable: false),
                    register_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<byte>(type: "smallint", nullable: false),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role_permissions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<long>(type: "bigint", nullable: false),
                    permission_id = table.Column<long>(type: "bigint", nullable: false),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_permissions", x => x.id);
                    table.ForeignKey(
                        name: "fk_role_permissions_permissions_permission_id",
                        column: x => x.permission_id,
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_permissions_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_profiles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    real_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    gender = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    birth_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    street = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    province = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    postal_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_profiles", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_profiles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    role_id = table.Column<long>(type: "bigint", nullable: false),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_security_logs",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    action = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ip_address = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    user_agent = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    browser = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    operating_system = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    device_type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    location = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    is_successful = table.Column<bool>(type: "boolean", nullable: false),
                    failure_reason = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_security_logs", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_security_logs_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_settings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    theme = table.Column<byte>(type: "smallint", nullable: false),
                    language = table.Column<byte>(type: "smallint", nullable: false),
                    time_zone = table.Column<byte>(type: "smallint", nullable: false),
                    enable_notifications = table.Column<bool>(type: "boolean", nullable: false),
                    enable_email_notifications = table.Column<bool>(type: "boolean", nullable: false),
                    enable_sms_notifications = table.Column<bool>(type: "boolean", nullable: false),
                    enable_two_factor_auth = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    date_format = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    time_format = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_settings", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_settings_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_role_permissions_permission_id",
                table: "role_permissions",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_permissions_role_id",
                table: "role_permissions",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_profiles_user_id",
                table: "user_profiles",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_role_id",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_user_id",
                table: "user_roles",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_security_logs_user_id",
                table: "user_security_logs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_settings_user_id",
                table: "user_settings",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "logs");

            migrationBuilder.DropTable(
                name: "role_permissions");

            migrationBuilder.DropTable(
                name: "user_profiles");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "user_security_logs");

            migrationBuilder.DropTable(
                name: "user_settings");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
