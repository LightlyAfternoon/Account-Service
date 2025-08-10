using Account_Service.Features.Accounts;
using Account_Service.Features.Accounts.UpdateAccount;
using Account_Service.Infrastructure.Mappers;
using MediatR;

namespace Account_Service.Features.Transactions.AddTransaction
{
    /// <inheritdoc />
    public class AddTransactionHandler : IRequestHandler<AddTransactionRequestCommand, TransactionDto?>
    {
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IAccountsService _accountService;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionsRepository"></param>
        /// <param name="accountService"></param>
        public AddTransactionHandler(ITransactionsRepository transactionsRepository, IAccountsService accountService)
        {
            _transactionsRepository = transactionsRepository;
            _accountService = accountService;
        }

        /// <inheritdoc />
        public async Task<TransactionDto?> Handle(AddTransactionRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            TransactionDto dto = new TransactionDto(id: Guid.Empty,
                accountId: requestCommand.AccountId,
                counterpartyAccountId: Guid.Empty,
                sum: requestCommand.Sum,
                currency: requestCommand.Currency,
                type: requestCommand.Type,
                description: requestCommand.Description,
                dateTime: requestCommand.DateTime);

            Transaction? transaction = await _transactionsRepository.Save(TransactionMappers.MapToEntity(dto));

            AccountDto? accountDto = await _accountService.FindById(requestCommand.AccountId);

            if (accountDto != null)
            {
                if (Enum.Parse<TransactionType>(requestCommand.Type).Equals(TransactionType.Credit))
                    accountDto.Balance -= requestCommand.Sum;
                else
                    accountDto.Balance += requestCommand.Sum;

                await _accountService.Update(accountDto.Id, new UpdateAccountRequestCommand(accountDto));
            }

            if (transaction != null)
            {
                return TransactionMappers.MapToDto(transaction);
            }

            return null;
        }
    }
}