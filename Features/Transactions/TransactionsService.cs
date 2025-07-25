using Account_Service.Features.Transactions.AddTransaction;
using Account_Service.Features.Transactions.AddTransferTransactions;
using Account_Service.Features.Transactions.DeleteTransaction;
using Account_Service.Features.Transactions.GetAccountStatementOnPeriod;
using Account_Service.Features.Transactions.TransactionsList;
using Account_Service.Features.Transactions.UpdateTransaction;
using MediatR;

namespace Account_Service.Features.Transactions
{
    public class TransactionsService : ITransactionsService
    {
        private readonly IMediator _mediator;

        public TransactionsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<List<TransactionDto>> FindAll()
        {
            return await _mediator.Send(new GetTransactionsListRequestCommand());
        }

        public async Task<TransactionDto?> Add(TransactionDto dto)
        {
            AddTransactionRequestCommand addTransactionRequestCommand = new()
            {
                Account = dto.Account,
                CounterpartyAccount = dto.CounterpartyAccount,
                Sum = dto.Sum,
                Currency = dto.Currency,
                Type = dto.Type,
                Description = dto.Description,
                DateTime = dto.DateTime
            };

            return await _mediator.Send(addTransactionRequestCommand);
        }

        public async Task<TransactionDto?> Update(Guid id, TransactionDto dto)
        {
            UpdateTransactionRequestCommand updateTransactionRequestCommand = new(id)
            {
                Account = dto.Account,
                CounterpartyAccount = dto.CounterpartyAccount,
                Sum = dto.Sum,
                Currency = dto.Currency,
                Type = dto.Type,
                Description = dto.Description,
                DateTime = dto.DateTime
            };

            return await _mediator.Send(updateTransactionRequestCommand);
        }

        public async Task<bool> DeleteById(Guid id)
        {
            return await _mediator.Send(new DeleteTransactionRequestCommand(id));
        }

        public async Task<List<TransactionDto>> GetAccountStatementOnPeriod(Guid accountId, DateTime startDate, DateTime endDate)
        {
            return await _mediator.Send(new GetAccountStatementOnPeriodRequestCommand(accountId, startDate, endDate));
        }

        public async Task<TransactionDto?> Transfer(Guid fromAccountId, Guid toAccountId, TransactionDto transactionDto)
        {
            AddTransferTransactionsRequestCommand addTransferTransactionsRequestCommand = new(fromAccountId, toAccountId)
            {
                Sum = transactionDto.Sum,
                Currency = transactionDto.Currency,
                Description = transactionDto.Description,
                DateTime = transactionDto.DateTime
            };

            return await _mediator.Send(addTransferTransactionsRequestCommand);
        }
    }
}