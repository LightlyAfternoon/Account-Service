using System.Data;
using System.Text.Json;
using Account_Service.Features.Accounts;
using Account_Service.Features.Accounts.AccrueInterest;
using Account_Service.Features.RabbitMQ;
using Account_Service.Features.Transactions.AddTransferTransactions.RabbitMQ;
using Account_Service.Infrastructure.Db;
using Account_Service.Infrastructure.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Account_Service.Features.Transactions.AddTransferTransactions
// ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public class AddTransferTransactionsHandler : IRequestHandler<AddTransferTransactionsRequestCommand, TransactionDto?>
    {
        private readonly ITransactionsRepository _transactionsRepository;
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
        public AddTransferTransactionsHandler(ITransactionsRepository transactionsRepository, IAccountsService accountService,
            IOutboxRepository outboxRepository, ApplicationContext applicationContext, IRabbitMqService rabbitMqService)
        {
            _transactionsRepository = transactionsRepository;
            _context = applicationContext;
            _outboxRepository = outboxRepository;
            _rabbitMqService = rabbitMqService;
        }

        /// <inheritdoc />
        public async Task<TransactionDto?> Handle(AddTransferTransactionsRequestCommand requestCommand,
            CancellationToken cancellationToken)
        {
            using var transaction =
                _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            try
            {
                var transactionFrom = await _transactionsRepository.MakeTransfer(requestCommand.FromAccountId,
                    requestCommand.ToAccountId, requestCommand, cancellationToken);

                if (transactionFrom == null)
                    return null;

                var dto = TransactionMappers.MapToDto(transactionFrom);
                var body = new TransferCompleted(eventId: Guid.NewGuid(), occurredAt: DateTime.UtcNow,
                    sourceAccountId: dto.AccountId,
                    destinationAccountId: (Guid)dto.CounterpartyAccountId!, amount: dto.Sum,
                    currency: dto.Currency, transferId: dto.Id,
                    new Meta(version: "v1", source: "Account Service",
                        correlationId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        causationId: Guid.Parse("22222222-2222-2222-2222-222222222222")));

                Outbox? outbox = new(Guid.Empty, "account.opened", nameof(AccrueInterestHandler),
                    JsonSerializer.Serialize(body));
                outbox = await _outboxRepository.Save(outbox, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                if (outbox != null) await _rabbitMqService.Publish(outbox, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                await (await transaction).CommitAsync(cancellationToken);

                return dto;

            }
            catch
            {
                await (await transaction).RollbackAsync(cancellationToken);

                throw new DbUpdateConcurrencyException();
            }
        }
    }
}