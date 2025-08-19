#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Account_Service.Migrations
{
    /// <inheritdoc />
    public partial class OutboxAndInbox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "inbox_consumed",
                columns: table => new
                {
                    message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    handler = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    payload = table.Column<string>(type: "character varying(655)", maxLength: 655, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inbox_consumed", x => x.message_id);
                });

            migrationBuilder.CreateTable(
                name: "inbox_dead_letters",
                columns: table => new
                {
                    message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    received_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    handler = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    payload = table.Column<string>(type: "character varying(655)", maxLength: 655, nullable: false),
                    error = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inbox_dead_letters", x => x.message_id);
                });

            migrationBuilder.CreateTable(
                name: "outbox_produced",
                columns: table => new
                {
                    message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    routing_key = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    handler = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    payload = table.Column<string>(type: "character varying(655)", maxLength: 655, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outbox_produced", x => x.message_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inbox_consumed");

            migrationBuilder.DropTable(
                name: "inbox_dead_letters");

            migrationBuilder.DropTable(
                name: "outbox_produced");
        }
    }
}
