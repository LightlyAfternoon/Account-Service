using System.Data;
using System.Text.Json;
using Account_Service.Features.Accounts.Antifraud.BlockAccount.RabbitMQ;
using Account_Service.Features.RabbitMQ;
using Account_Service.Infrastructure.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Account_Service.Features.Accounts.Antifraud.UnblockAccount
{
    /// <inheritdoc />
    public class UnblockAccountHandler : IRequestHandler<UnblockAccountRequestCommand>
    {
        private readonly IAccountsRepository _accountsRepository;
        private readonly ApplicationContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountsRepository"></param>
        /// <param name="applicationContext"></param>
        /// <param name="rabbitMqService"></param>
        public UnblockAccountHandler(IAccountsRepository accountsRepository, ApplicationContext applicationContext,
            IRabbitMqService rabbitMqService)
        {
            _accountsRepository = accountsRepository;
            _context = applicationContext;
        }

        /// <inheritdoc />
        public async Task Handle(UnblockAccountRequestCommand requestCommand,
            CancellationToken cancellationToken)
        {
            using var transaction =
                _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            try
            {
                var body = JsonSerializer.Deserialize<Inbox>(requestCommand.Message);
                var meta = JsonSerializer.Deserialize<Meta>(requestCommand.Message);
                if (body != null &&
                    await _context.Inboxes.FirstOrDefaultAsync(b => b.MessageId.Equals(body.MessageId), cancellationToken: cancellationToken) == null)
                {
                    if (meta is { Version: "v1" })
                    {
                        var payload = JsonSerializer.Deserialize<ClientBlockedPayload>(requestCommand.Message);
                        if (payload != null)
                        {
                            await _accountsRepository.UnfrozeAllUserAccounts(payload.ClientId, cancellationToken);
                        }

                        body.ProcessedAt = DateTime.Now;
                        await _context.Inboxes.AddAsync(body, cancellationToken);
                    }
                    else
                    {
                        var deadLetter = new InboxDeadLetters(body, "Version not supported");
                        await _context.InboxDeadLetters.AddAsync(deadLetter, cancellationToken);
                    }

                    await _context.SaveChangesAsync(cancellationToken);

                    await (await transaction).CommitAsync(cancellationToken);
                }
                else
                {
                    await (await transaction).RollbackAsync(cancellationToken);
                }
            }
            catch
            {
                await (await transaction).RollbackAsync(cancellationToken);

                throw new DbUpdateConcurrencyException();
            }
        }
    }
}