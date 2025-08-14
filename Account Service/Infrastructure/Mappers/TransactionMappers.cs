using Account_Service.Features.Accounts;
using Account_Service.Features.Transactions;

namespace Account_Service.Infrastructure.Mappers
{
    /// <summary>
    /// 
    /// </summary>
    public class TransactionMappers
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static TransactionDto MapToDto(Transaction transaction) => new(id: transaction.Id,
            accountId: transaction.AccountId,
            counterpartyAccountId: transaction.CounterpartyAccountId,
            sum: transaction.Sum,
            currency: transaction.Currency.ToString(),
            type: transaction.Type.ToString(),
            description: transaction.Description,
            dateTime: transaction.DateTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionDto"></param>
        /// <returns></returns>
        public static Transaction MapToEntity(TransactionDto transactionDto) => new(id: transactionDto.Id,
            accountId: transactionDto.AccountId,
            counterpartyAccountId: transactionDto.CounterpartyAccountId,
            sum: transactionDto.Sum,
            currency: Enum.Parse<CurrencyCode>(transactionDto.Currency),
            type: Enum.Parse<TransactionType>(transactionDto.Type),
            description: transactionDto.Description,
            dateTime: transactionDto.DateTime);
    }
}