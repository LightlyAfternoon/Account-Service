using System.Data;
using System.Text.Json;
using Account_Service.Features.Accounts.AddAccount.RabbitMQ;
using Account_Service.Features.RabbitMQ;
using Account_Service.Infrastructure.Db;
using Account_Service.Infrastructure.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Account_Service.Features.Accounts.AddAccount
// ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public class AddAccountHandler : IRequestHandler<AddAccountRequestCommand, AccountDto?>
    {
        private readonly IAccountsRepository _accountsRepository;
        private readonly ApplicationContext _context;
        private readonly IOutboxRepository _outboxRepository;
        private readonly IRabbitMqService _rabbitMqService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountsRepository"></param>
        /// <param name="applicationContext"></param>
        /// <param name="outboxRepository"></param>
        /// <param name="rabbitMqService"></param>=
        public AddAccountHandler(IAccountsRepository accountsRepository, ApplicationContext applicationContext,
            IOutboxRepository outboxRepository, IRabbitMqService rabbitMqService)
        {
            _accountsRepository = accountsRepository;
            _context = applicationContext;
            _outboxRepository = outboxRepository;
            _rabbitMqService = rabbitMqService;
        }

        /// <inheritdoc />
        public async Task<AccountDto?> Handle(AddAccountRequestCommand requestCommand,
            CancellationToken cancellationToken)
        {
            using var transaction =
                _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            try
            {
                var dto = new AccountDto(id: Guid.Empty,
                    ownerId: requestCommand.OwnerId,
                    type: requestCommand.Type,
                    currency: requestCommand.Currency,
                    balance: requestCommand.Balance,
                    interestRate: requestCommand.InterestRate,
                    openDate: requestCommand.OpenDate,
                    closeDate: requestCommand.CloseDate);

                var account = await _accountsRepository.Save(AccountMappers.MapToEntity(dto), cancellationToken);

                if (account == null)
                    return null;

                var body = new AccountOpened(eventId: Guid.NewGuid(), occurredAt: DateTime.UtcNow,
                    ownerId: dto.OwnerId, currency: dto.Currency, type: dto.Type,
                    new Meta(version: "v1", source: "Account Service",
                        correlationId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        causationId: Guid.Parse("22222222-2222-2222-2222-222222222222")));

                Outbox? outbox = new(Guid.Empty, "account.opened", GetType().Name,
                    JsonSerializer.Serialize(body));
                outbox = await _outboxRepository.Save(outbox, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                if (outbox != null) await _rabbitMqService.Publish(outbox, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                await (await transaction).CommitAsync(cancellationToken);

                return AccountMappers.MapToDto(account);

            }
            catch
            {
                await (await transaction).RollbackAsync(cancellationToken);

                throw new DbUpdateConcurrencyException();
            }
        }
    }
}