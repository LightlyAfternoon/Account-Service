using Account_Service.Features.Accounts;
using Account_Service.Features.Accounts.AccrueInterest;
using Account_Service.Features.Accounts.UpdateAccount;
using Account_Service.Features.RabbitMQ;
using Account_Service.Features.Transactions.AddTransaction.RabbitMQ;
using Account_Service.Infrastructure.Db;
using Account_Service.Infrastructure.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

namespace Account_Service.Features.Transactions.AddTransaction
{
    /// <inheritdoc />
    public class AddTransactionHandler : IRequestHandler<AddTransactionRequestCommand, TransactionDto?>
    {
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IAccountsService _accountService;
        private readonly ApplicationContext _context;
        private readonly IOutboxRepository _outboxRepository;
        private readonly IRabbitMqService _rabbitMqService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionsRepository"></param>
        /// <param name="accountService"></param>
        /// <param name="outboxRepository"></param>
        /// <param name="applicationContext"></param>
        /// <param name="rabbitMqService"></param>
        public AddTransactionHandler(ITransactionsRepository transactionsRepository, IAccountsService accountService,
            IOutboxRepository outboxRepository, ApplicationContext applicationContext, IRabbitMqService rabbitMqService)
        {
            _transactionsRepository = transactionsRepository;
            _accountService = accountService;
            _context = applicationContext;
            _outboxRepository = outboxRepository;
            _rabbitMqService = rabbitMqService;
        }

        /// <inheritdoc />
        public async Task<TransactionDto?> Handle(AddTransactionRequestCommand requestCommand,
            CancellationToken cancellationToken)
        {
            using var transaction =
                _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            try
            {
                TransactionDto dto = new TransactionDto(id: Guid.Empty,
                    accountId: requestCommand.AccountId,
                    counterpartyAccountId: null,
                    sum: requestCommand.Sum,
                    currency: requestCommand.Currency,
                    type: requestCommand.Type,
                    description: requestCommand.Description,
                    dateTime: requestCommand.DateTime);

                Transaction? accountTransaction =
                    await _transactionsRepository.Save(TransactionMappers.MapToEntity(dto), cancellationToken);

                AccountDto? accountDto = await _accountService.FindById(requestCommand.AccountId);

                if (accountDto != null)
                {
                    if (Enum.Parse<TransactionType>(requestCommand.Type).Equals(TransactionType.Credit))
                        accountDto.Balance -= requestCommand.Sum;
                    else
                        accountDto.Balance += requestCommand.Sum;

                    await _accountService.Update(accountDto.Id, new UpdateAccountRequestCommand(accountDto));
                }

                if (accountTransaction != null)
                {
                    object? body = null;
                    if (dto.Type.Equals(nameof(TransactionType.Credit)))
                    {
                        body = new MoneyCredited(eventId: Guid.NewGuid(), occurredAt: DateTime.Now,
                            ownerId: dto.AccountId, amount: dto.Sum,
                            currency: dto.Currency, operationId: dto.Id,
                            new Meta(version: "v1", source: "Account Service",
                                correlationId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                                causationId: Guid.Parse("22222222-2222-2222-2222-222222222222")));
                    }
                    else if (dto.Type.Equals(nameof(TransactionType.Debit)))
                    {
                        body = new MoneyDebited(eventId: Guid.NewGuid(), occurredAt: DateTime.Now,
                            ownerId: dto.AccountId, amount: dto.Sum,
                            currency: dto.Currency, operationId: dto.Id,
                            new Meta(version: "v1", source: "Account Service",
                                correlationId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                                causationId: Guid.Parse("22222222-2222-2222-2222-222222222222")));
                    }

                    if (body != null)
                    {
                        Outbox outbox = new(Guid.Empty, "account.opened", nameof(AccrueInterestHandler),
                            JsonSerializer.Serialize(body));
                        await _outboxRepository.Save(outbox, cancellationToken);

                        await _rabbitMqService.Publish(outbox);

                        await _context.SaveChangesAsync(cancellationToken);

                        await(await transaction).CommitAsync(cancellationToken);

                        return TransactionMappers.MapToDto(accountTransaction);
                    }
                }

                return null;
            }
            catch
            {
                await(await transaction).RollbackAsync(cancellationToken);

                throw new DbUpdateConcurrencyException();
            }
        }
    }
}