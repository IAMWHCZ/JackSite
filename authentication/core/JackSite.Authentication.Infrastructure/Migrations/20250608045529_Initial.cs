using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JackSite.Authentication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "action_basics",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    action_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    action_description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    order = table.Column<int>(type: "int", nullable: false),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_action_basics", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "email_basics",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    type = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    message_id = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    sender_email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    sender_name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    importance = table.Column<int>(type: "int", nullable: false),
                    sent_time = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    last_try_time = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    retry_count = table.Column<int>(type: "int", nullable: false),
                    failure_reason = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    is_draft = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    drafted_on_utc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_email_basics", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "email_templates",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    description = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true),
                    subject = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    body = table.Column<string>(type: "longtext", nullable: false),
                    is_html = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    parameters = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true),
                    category = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    version = table.Column<int>(type: "int", nullable: false),
                    is_system = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    last_used_time = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    usage_count = table.Column<int>(type: "int", nullable: false),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    is_draft = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    drafted_on_utc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_email_templates", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "operation_logs",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    api_name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    description = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true),
                    is_authorization = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    ip_address = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    user_agent = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true),
                    browser = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    os = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    status_code = table.Column<int>(type: "int", nullable: false),
                    exception = table.Column<string>(type: "text", nullable: true),
                    elapsed_milliseconds = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_operation_logs", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "permission_models",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    fil_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    is_deleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    deleted_on_utc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permission_models", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "resources",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    resource_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    path = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    parent_id = table.Column<long>(type: "bigint", nullable: false),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_resources", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    role_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "translations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    key_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    english_text = table.Column<string>(type: "longtext", nullable: true),
                    chinese_text = table.Column<string>(type: "longtext", nullable: true),
                    category = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_translations", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "email_attachments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    object_key = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    description = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true),
                    file_name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    file_size = table.Column<long>(type: "bigint", nullable: false),
                    content_type = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    is_inline = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    content_id = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    file_extension = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true),
                    upload_time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    storage_type = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    email_record_id = table.Column<long>(type: "bigint", nullable: false),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    is_draft = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    drafted_on_utc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_email_attachments", x => x.id);
                    table.ForeignKey(
                        name: "fk_email_attachments_email_basics_email_basic_id",
                        column: x => x.email_record_id,
                        principalTable: "email_basics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "email_contents",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    content = table.Column<string>(type: "longtext", nullable: true),
                    is_html = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    subject = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true),
                    plain_text_content = table.Column<string>(type: "longtext", nullable: true),
                    recipient = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true),
                    cc = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true),
                    bcc = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true),
                    template_id = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    template_parameters = table.Column<string>(type: "longtext", nullable: true),
                    preview_text = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true),
                    email_id = table.Column<long>(type: "bigint", nullable: false),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    is_draft = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    drafted_on_utc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_email_contents", x => x.id);
                    table.ForeignKey(
                        name: "fk_email_basics_email_contents_email_content_id",
                        column: x => x.email_id,
                        principalTable: "email_basics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "email_recipients",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    recipient_email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    recipient_name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    type = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    delivered_time = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    read_time = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    tracking_id = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    failure_reason = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true),
                    email_record_id = table.Column<long>(type: "bigint", nullable: false),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    is_draft = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    drafted_on_utc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_email_recipients", x => x.id);
                    table.ForeignKey(
                        name: "fk_email_recipients_email_basics_email_basic_id",
                        column: x => x.email_record_id,
                        principalTable: "email_basics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "permission_policies",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    resource_id = table.Column<long>(type: "bigint", nullable: false),
                    model_id = table.Column<long>(type: "bigint", nullable: false),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permission_policies", x => x.id);
                    table.ForeignKey(
                        name: "fk_permission_policies_permission_models_model_id",
                        column: x => x.model_id,
                        principalTable: "permission_models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_permission_policies_resources_resource_id",
                        column: x => x.resource_id,
                        principalTable: "resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_groups",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    group_name = table.Column<string>(type: "longtext", nullable: false),
                    description = table.Column<string>(type: "longtext", nullable: true),
                    role_id = table.Column<long>(type: "bigint", nullable: true),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_groups", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_groups_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "permission_policy_actions",
                columns: table => new
                {
                    ActionBasicsId = table.Column<long>(type: "bigint", nullable: false),
                    PermissionPolicyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission_policy_actions", x => new { x.ActionBasicsId, x.PermissionPolicyId });
                    table.ForeignKey(
                        name: "FK_permission_policy_actions_action_basics_ActionBasicsId",
                        column: x => x.ActionBasicsId,
                        principalTable: "action_basics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_permission_policy_actions_permission_policies_PermissionPoli~",
                        column: x => x.PermissionPolicyId,
                        principalTable: "permission_policies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "permission_policy_conditions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    filter = table.Column<string>(type: "longtext", nullable: false),
                    data_source = table.Column<string>(type: "longtext", nullable: false),
                    PermissionPolicyId = table.Column<long>(type: "bigint", nullable: true),
                    PolicyId = table.Column<long>(type: "bigint", nullable: true),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permission_policy_conditions", x => x.id);
                    table.ForeignKey(
                        name: "FK_permission_policy_conditions_permission_policies_PermissionP~",
                        column: x => x.PermissionPolicyId,
                        principalTable: "permission_policies",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_permission_policy_conditions_permission_policies_permission_~",
                        column: x => x.PolicyId,
                        principalTable: "permission_policies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "role_references",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    role_id = table.Column<long>(type: "bigint", nullable: false),
                    group_id = table.Column<long>(type: "bigint", nullable: false),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_references", x => x.id);
                    table.ForeignKey(
                        name: "FK_role_references_user_groups_group_id",
                        column: x => x.group_id,
                        principalTable: "user_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_references_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_basics",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    salt = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    is_deleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    deleted_on_utc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    phone_number = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    email_confirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    avatar_url = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    last_login_time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    last_login_ip = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    login_count = table.Column<int>(type: "int", nullable: false),
                    register_time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    status = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    UserProfileId = table.Column<long>(type: "bigint", nullable: true),
                    UserSettingsId = table.Column<long>(type: "bigint", nullable: true),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_basics", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_group_references",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    user_group_id = table.Column<long>(type: "bigint", nullable: false),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_group_references", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_group_references_user_basics_user_id",
                        column: x => x.user_id,
                        principalTable: "user_basics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_group_references_user_groups_user_group_id",
                        column: x => x.user_group_id,
                        principalTable: "user_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_profiles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    real_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    gender = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    birth_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    street = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    city = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    province = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    country = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    postal_code = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    is_deleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    deleted_on_utc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_profiles", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_profiles_user_basics_user_id",
                        column: x => x.user_id,
                        principalTable: "user_basics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_security_logs",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    action = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ip_address = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    user_agent = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    browser = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    operating_system = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    device_type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    location = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    is_successful = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    failure_reason = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId1 = table.Column<long>(type: "bigint", nullable: false),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_security_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_security_logs_user_basics_UserId1",
                        column: x => x.UserId1,
                        principalTable: "user_basics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_security_logs_user_basics_user_id",
                        column: x => x.user_id,
                        principalTable: "user_basics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_settings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    theme = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    language = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    time_zone = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    enable_notifications = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    enable_email_notifications = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    enable_sms_notifications = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    enable_two_factor_auth = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    two_factor_type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    date_format = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    time_format = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    create_by = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    update_by = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_settings", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_settings_user_basics_user_id",
                        column: x => x.user_id,
                        principalTable: "user_basics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_email_attachments_email_record_id",
                table: "email_attachments",
                column: "email_record_id");

            migrationBuilder.CreateIndex(
                name: "IX_email_contents_email_id",
                table: "email_contents",
                column: "email_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_email_recipients_email_record_id",
                table: "email_recipients",
                column: "email_record_id");

            migrationBuilder.CreateIndex(
                name: "IX_email_templates_name_version",
                table: "email_templates",
                columns: new[] { "name", "version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_permission_policies_model_id",
                table: "permission_policies",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "ix_permission_policies_resource_id",
                table: "permission_policies",
                column: "resource_id");

            migrationBuilder.CreateIndex(
                name: "IX_permission_policy_actions_PermissionPolicyId",
                table: "permission_policy_actions",
                column: "PermissionPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_permission_policy_conditions_PermissionPolicyId",
                table: "permission_policy_conditions",
                column: "PermissionPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_permission_policy_conditions_PolicyId",
                table: "permission_policy_conditions",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_role_references_group_id",
                table: "role_references",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_references_role_id",
                table: "role_references",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_basics_email",
                table: "user_basics",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_basics_username",
                table: "user_basics",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_basics_UserProfileId",
                table: "user_basics",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_user_basics_UserSettingsId",
                table: "user_basics",
                column: "UserSettingsId");

            migrationBuilder.CreateIndex(
                name: "ix_user_group_references_user_group_id",
                table: "user_group_references",
                column: "user_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_group_references_user_id",
                table: "user_group_references",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_groups_role_id",
                table: "user_groups",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_profiles_user_id",
                table: "user_profiles",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_security_logs_user_id",
                table: "user_security_logs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_security_logs_UserId1",
                table: "user_security_logs",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "ix_user_settings_user_id",
                table: "user_settings",
                column: "user_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_user_basics_user_profiles_UserProfileId",
                table: "user_basics",
                column: "UserProfileId",
                principalTable: "user_profiles",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_basics_user_settings_UserSettingsId",
                table: "user_basics",
                column: "UserSettingsId",
                principalTable: "user_settings",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_basics_user_profiles_UserProfileId",
                table: "user_basics");

            migrationBuilder.DropForeignKey(
                name: "FK_user_basics_user_settings_UserSettingsId",
                table: "user_basics");

            migrationBuilder.DropTable(
                name: "email_attachments");

            migrationBuilder.DropTable(
                name: "email_contents");

            migrationBuilder.DropTable(
                name: "email_recipients");

            migrationBuilder.DropTable(
                name: "email_templates");

            migrationBuilder.DropTable(
                name: "operation_logs");

            migrationBuilder.DropTable(
                name: "permission_policy_actions");

            migrationBuilder.DropTable(
                name: "permission_policy_conditions");

            migrationBuilder.DropTable(
                name: "role_references");

            migrationBuilder.DropTable(
                name: "translations");

            migrationBuilder.DropTable(
                name: "user_group_references");

            migrationBuilder.DropTable(
                name: "user_security_logs");

            migrationBuilder.DropTable(
                name: "email_basics");

            migrationBuilder.DropTable(
                name: "action_basics");

            migrationBuilder.DropTable(
                name: "permission_policies");

            migrationBuilder.DropTable(
                name: "user_groups");

            migrationBuilder.DropTable(
                name: "permission_models");

            migrationBuilder.DropTable(
                name: "resources");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "user_profiles");

            migrationBuilder.DropTable(
                name: "user_settings");

            migrationBuilder.DropTable(
                name: "user_basics");
        }
    }
}
