using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Account_Service.Migrations
{
    /// <inheritdoc />
    public partial class AddNewUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[]{ "id", "name" },
                values: new object[,]
                {
                    {"3fa85f64-5717-4562-b3fc-2c963f66afa6", "Иван"}
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id",
                keyValue: "3fa85f64-5717-4562-b3fc-2c963f66afa6");
        }
    }
}
