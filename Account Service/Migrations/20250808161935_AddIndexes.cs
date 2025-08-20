#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Account_Service.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_accountId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_ownerId",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "dateTime",
                table: "Transactions",
                newName: "date");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_accountId_date",
                table: "Transactions",
                columns: new[] { "accountId", "date" });

            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS btree_gist;");
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_date",
                table: "Transactions",
                column: "date")
                .Annotation("Npgsql:IndexMethod", "gist");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ownerId",
                table: "Accounts",
                column: "ownerId")
                .Annotation("Npgsql:IndexMethod", "hash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_accountId_date",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_date",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_ownerId",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Transactions",
                newName: "dateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_accountId",
                table: "Transactions",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ownerId",
                table: "Accounts",
                column: "ownerId");
        }
    }
}
