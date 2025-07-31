using Account_Service.Features.Accounts;
using Account_Service.Features.Accounts.UpdateAccount;
using Account_Service.Infrastructure.Mappers;
using MediatR;

namespace Account_Service.Features.Transactions.AddTransferTransactions
{
    /// <inheritdoc />
    public class AddTransferTransactionsHandler : IRequestHandler<AddTransferTransactionsRequestCommand, TransactionDto?>
    {
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IAccountService _accountService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionsRepository"></param>
        /// <param name="accountService"></param>
        public AddTransferTransactionsHandler(ITransactionsRepository transactionsRepository, IAccountService accountService)
        {
            _transactionsRepository = transactionsRepository;
            _accountService = accountService;
        }

        /// <inheritdoc />
        public async Task<TransactionDto?> Handle(AddTransferTransactionsRequestCommand requestCommand,
            CancellationToken cancellationToken)
        {
            Transaction? transactionFrom = new Transaction(id: Guid.Empty,
                accountId: requestCommand.FromAccountId,
                counterpartyAccountId: requestCommand.ToAccountId,
                sum: requestCommand.Sum,
                currency: Enum.Parse<CurrencyCode>(requestCommand.Currency),
                type: TransactionType.Credit,
                description: requestCommand.Description,
                dateTime: requestCommand.DateTime);

            Transaction transactionTo = new Transaction(id: Guid.Empty,
                accountId: requestCommand.ToAccountId,
                counterpartyAccountId: requestCommand.FromAccountId,
                sum: requestCommand.Sum,
                currency: Enum.Parse<CurrencyCode>(requestCommand.Currency),
                type: TransactionType.Debit,
                description: requestCommand.Description,
                dateTime: requestCommand.DateTime);

            AccountDto? accountDtoFrom = await _accountService.FindById(requestCommand.FromAccountId);
            AccountDto? accountDtoTo = await _accountService.FindById(requestCommand.ToAccountId);

            if (accountDtoFrom != null)
                accountDtoFrom.Balance -= requestCommand.Sum;
            if (accountDtoTo != null)
                accountDtoTo.Balance += requestCommand.Sum;

            await _transactionsRepository.Save(transactionTo);
            if ((transactionFrom = await _transactionsRepository.Save(transactionFrom)) != null)
            {
                if (accountDtoFrom != null)
                    await _accountService.Update(accountDtoFrom.Id, new UpdateAccountRequestCommand(accountDtoFrom));
                if (accountDtoTo != null)
                    await _accountService.Update(accountDtoTo.Id, new UpdateAccountRequestCommand(accountDtoTo));

                return TransactionMappers.MapToDto(transactionFrom);
            }

            return null;
        }
    }
}