using Account_Service.Features.Transactions.AddTransaction;
using Account_Service.Features.Transactions.AddTransferTransactions;
using Account_Service.Features.Transactions.GetAccountStatementOnPeriod;
using MediatR;

namespace Account_Service.Features.Transactions
{
    /// <inheritdoc />
    public class TransactionsService : ITransactionsService
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public TransactionsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<TransactionDto?> Add(AddTransactionRequestCommand requestCommand)
        {
            AddTransactionRequestCommand addTransactionRequestCommand = new(accountId: requestCommand.AccountId,
                sum: requestCommand.Sum,
                currency: requestCommand.Currency,
                type: requestCommand.Type,
                description: requestCommand.Description,
                dateTime: requestCommand.DateTime);

            return await _mediator.Send(addTransactionRequestCommand);
        }

        /// <inheritdoc />
        public async Task<List<TransactionDto>> GetAccountStatementOnPeriod(Guid accountId, DateTime startDate, DateTime endDate)
        {
            return await _mediator.Send(new GetAccountStatementOnPeriodRequestCommand(accountId, startDate, endDate));
        }

        /// <inheritdoc />
        public async Task<TransactionDto?> Transfer(Guid fromAccountId, Guid toAccountId, AddTransferTransactionsRequestCommand requestCommand)
        {
            requestCommand.FromAccountId = fromAccountId;
            requestCommand.ToAccountId = toAccountId;

            return await _mediator.Send(requestCommand);
        }
    }
}