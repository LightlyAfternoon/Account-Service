using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Account_Service.Migrations
{
    /// <inheritdoc />
    public partial class AddRowVersionColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "rowVersion",
                table: "Users",
                type: "integer",
                rowVersion: true,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rowVersion",
                table: "Transactions",
                type: "integer",
                rowVersion: true,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rowVersion",
                table: "Accounts",
                type: "integer",
                rowVersion: true,
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rowVersion",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "rowVersion",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "rowVersion",
                table: "Accounts");
        }
    }
}
