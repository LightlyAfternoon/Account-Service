using Account_Service.Features.Transactions;

namespace Account_Service.Infrastructure
{
    /// <inheritdoc />
    public class TransactionMappers : IMappers<TransactionDto, Transaction>
    {
        /// <inheritdoc />
        public static TransactionDto MapToDto(Transaction transaction) => new(id: transaction.Id,
            accountId: transaction.AccountId,
            counterpartyAccountId: transaction.CounterpartyAccountId,
            sum: transaction.Sum,
            currency: transaction.Currency,
            type: transaction.Type,
            description: transaction.Description,
            dateTime: transaction.DateTime);

        /// <inheritdoc />
        public static Transaction MapToEntity(TransactionDto transactionDto) => new(id: transactionDto.Id,
            accountId: transactionDto.AccountId,
            counterpartyAccountId: transactionDto.CounterpartyAccountId,
            sum: transactionDto.Sum,
            currency: transactionDto.Currency,
            type: transactionDto.Type,
            description: transactionDto.Description,
            dateTime: transactionDto.DateTime);
    }
}