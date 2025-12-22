using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hubs.API.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "hubs_service");

            migrationBuilder.CreateTable(
                name: "hubs",
                schema: "hubs_service",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hubs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "channels",
                schema: "hubs_service",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    hub_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_channels", x => x.id);
                    table.ForeignKey(
                        name: "FK_channels_hubs_hub_id",
                        column: x => x.hub_id,
                        principalSchema: "hubs_service",
                        principalTable: "hubs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "hub_memberships",
                schema: "hubs_service",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    hub_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hub_memberships", x => x.id);
                    table.ForeignKey(
                        name: "FK_hub_memberships_hubs_hub_id",
                        column: x => x.hub_id,
                        principalSchema: "hubs_service",
                        principalTable: "hubs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "channel_memberships",
                schema: "hubs_service",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    channel_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_channel_memberships", x => x.id);
                    table.ForeignKey(
                        name: "FK_channel_memberships_channels_channel_id",
                        column: x => x.channel_id,
                        principalSchema: "hubs_service",
                        principalTable: "channels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                schema: "hubs_service",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    channel_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.id);
                    table.ForeignKey(
                        name: "FK_messages_channels_channel_id",
                        column: x => x.channel_id,
                        principalSchema: "hubs_service",
                        principalTable: "channels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_channel_memberships_channel_id_user_id",
                schema: "hubs_service",
                table: "channel_memberships",
                columns: new[] { "channel_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_channel_memberships_id",
                schema: "hubs_service",
                table: "channel_memberships",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_channels_hub_id",
                schema: "hubs_service",
                table: "channels",
                column: "hub_id");

            migrationBuilder.CreateIndex(
                name: "IX_channels_id",
                schema: "hubs_service",
                table: "channels",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_hub_memberships_hub_id_user_id",
                schema: "hubs_service",
                table: "hub_memberships",
                columns: new[] { "hub_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_hub_memberships_id",
                schema: "hubs_service",
                table: "hub_memberships",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_hubs_id",
                schema: "hubs_service",
                table: "hubs",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_messages_channel_id",
                schema: "hubs_service",
                table: "messages",
                column: "channel_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "channel_memberships",
                schema: "hubs_service");

            migrationBuilder.DropTable(
                name: "hub_memberships",
                schema: "hubs_service");

            migrationBuilder.DropTable(
                name: "messages",
                schema: "hubs_service");

            migrationBuilder.DropTable(
                name: "channels",
                schema: "hubs_service");

            migrationBuilder.DropTable(
                name: "hubs",
                schema: "hubs_service");
        }
    }
}
