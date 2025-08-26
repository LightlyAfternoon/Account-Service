using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Account_Service.Migrations
{
    /// <inheritdoc />
    public partial class AddProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // в плане вкладов работает скорее как сложные проценты для накопительного счёта, для более сложной логики нужны доп свойства для счетов
            migrationBuilder.Sql("CREATE PROCEDURE accrue_interest(account_id UUID)\r\n" +
                                 "LANGUAGE plpgsql\r\n" +
                                 "AS $$\r\n" +
                                 "BEGIN\r\n" +
                                 "    UPDATE \"Accounts\" SET balance = balance + (balance * (\"interestRate\" / 100) / 365) WHERE id = account_id;\r\n" +
                                 "    COMMIT;\r\n" +
                                 "END;\r\n" +
                                 "$$;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE accrue_interest");
        }
    }
}
