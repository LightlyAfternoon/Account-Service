using System.Data;
using System.Text.Json;
using Account_Service.Features.Accounts.AccrueInterest.RabbitMQ;
using Account_Service.Features.RabbitMQ;
using Account_Service.Infrastructure.Db;
using Account_Service.Infrastructure.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Account_Service.Features.Accounts.AccrueInterest
    // ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public class AccrueInterestHandler : IRequestHandler<AccrueInterestRequestCommand, List<AccountDto>>
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
        /// <param name="rabbitMqService"></param>
        public AccrueInterestHandler(IAccountsRepository accountsRepository, ApplicationContext applicationContext,
            IOutboxRepository outboxRepository, IRabbitMqService rabbitMqService)
        {
            _accountsRepository = accountsRepository;
            _context = applicationContext;
            _outboxRepository = outboxRepository;
            _rabbitMqService = rabbitMqService;
        }

        /// <inheritdoc />
        public async Task<List<AccountDto>> Handle(AccrueInterestRequestCommand request, CancellationToken cancellationToken)
        {
            using var transaction =
                _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            try
            {
                // ReSharper disable once IdentifierTypo
                var accountDtos = (await _accountsRepository.AccrueInterestForAllOpenedAccounts(cancellationToken))
                    .Select(AccountMappers.MapToDto).ToList();

                foreach (var outbox in accountDtos.Select(accountDto => new InterestAccrued(eventId: Guid.NewGuid(), occurredAt: DateTime.UtcNow,
                             accountId: accountDto.OwnerId, periodFrom: DateTime.Today.AddDays(-1), periodTo: DateTime.Today,
                             amount: (decimal)(accountDto.Balance +
                                               accountDto.Balance * (accountDto.InterestRate / 100) / 365)!,
                             new Meta(version: "v1", source: "Account Service",
                                 correlationId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                                 causationId: Guid.Parse("22222222-2222-2222-2222-222222222222")))).Select(body => new Outbox(Guid.Empty, "money.credited", nameof(AccrueInterestHandler),
                             JsonSerializer.Serialize(body))))
                {
                    await _outboxRepository.Save(outbox, cancellationToken);

                    await _rabbitMqService.Publish(outbox, cancellationToken);
                }

                await _context.SaveChangesAsync(cancellationToken);

                await (await transaction).CommitAsync(cancellationToken);
                
                return accountDtos;
            }
            catch
            {
                await (await transaction).RollbackAsync(cancellationToken);

                throw new DbUpdateConcurrencyException();
            }
        }
    }
}