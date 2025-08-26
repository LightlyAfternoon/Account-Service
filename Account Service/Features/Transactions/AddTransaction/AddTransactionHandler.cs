using System.Data;
using System.Text.Json;
using Account_Service.Features.Accounts;
using Account_Service.Features.Accounts.UpdateAccount;
using Account_Service.Features.RabbitMQ;
using Account_Service.Features.Transactions.AddTransaction.RabbitMQ;
using Account_Service.Infrastructure.Db;
using Account_Service.Infrastructure.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Account_Service.Features.Transactions.AddTransaction
// ReSharper disable once ArrangeNamespaceBody
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
                var dto = new TransactionDto(id: Guid.Empty,
                    accountId: requestCommand.AccountId,
                    counterpartyAccountId: null,
                    sum: requestCommand.Sum,
                    currency: requestCommand.Currency,
                    type: requestCommand.Type,
                    description: requestCommand.Description,
                    dateTime: requestCommand.DateTime);

                var accountTransaction =
                    await _transactionsRepository.Save(TransactionMappers.MapToEntity(dto), cancellationToken);

                var accountDto = await _accountService.FindById(requestCommand.AccountId);

                if (accountDto != null)
                {
                    if (Enum.Parse<TransactionType>(requestCommand.Type).Equals(TransactionType.Debit))
                        accountDto.Balance -= requestCommand.Sum;
                    else if (Enum.Parse<TransactionType>(requestCommand.Type).Equals(TransactionType.Credit))
                        accountDto.Balance += requestCommand.Sum;

                    await _accountService.Update(accountDto.Id, new UpdateAccountRequestCommand(accountDto));
                }

                if (accountTransaction == null)
                    return null;

                object? body = dto.Type switch
                {
                    nameof(TransactionType.Credit) => new MoneyCredited(eventId: Guid.NewGuid(),
                        occurredAt: DateTime.UtcNow, ownerId: dto.AccountId, amount: dto.Sum, currency: dto.Currency,
                        operationId: dto.Id,
                        new Meta(version: "v1", source: "Account Service",
                            correlationId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                            causationId: Guid.Parse("22222222-2222-2222-2222-222222222222"))),
                    nameof(TransactionType.Debit) => new MoneyDebited(eventId: Guid.NewGuid(),
                        occurredAt: DateTime.UtcNow, ownerId: dto.AccountId, amount: dto.Sum, currency: dto.Currency,
                        operationId: dto.Id,
                        new Meta(version: "v1", source: "Account Service",
                            correlationId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                            causationId: Guid.Parse("22222222-2222-2222-2222-222222222222"))),
                    _ => null
                };

                if (body == null)
                    return null;

                Outbox? outbox = new(Guid.Empty, "account.opened", GetType().Name,
                    JsonSerializer.Serialize(body));
                outbox = await _outboxRepository.Save(outbox, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                if (outbox != null) await _rabbitMqService.Publish(outbox, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                await(await transaction).CommitAsync(cancellationToken);

                return TransactionMappers.MapToDto(accountTransaction);

            }
            catch
            {
                await(await transaction).RollbackAsync(cancellationToken);

                throw new DbUpdateConcurrencyException();
            }
        }
    }
}