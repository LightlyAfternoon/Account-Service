using System.Data;
using Account_Service.Features.Accounts.AccrueInterest;
using Account_Service.Features.Accounts.Antifraud.BlockAccount.RabbitMQ;
using Account_Service.Features.RabbitMQ;
using Account_Service.Infrastructure.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Account_Service.Features.Accounts.Antifraud.BlockAccount
// ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public class BlockAccountHandler : IRequestHandler<BlockAccountRequestCommand>
    {
        private readonly IAccountsRepository _accountsRepository;
        private readonly ApplicationContext _context;
        private readonly ILogger<AccrueInterestHandler> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountsRepository"></param>
        /// <param name="applicationContext"></param>
        /// <param name="rabbitMqService"></param>
        /// <param name="logger"></param>
        public BlockAccountHandler(IAccountsRepository accountsRepository, ApplicationContext applicationContext,
            IRabbitMqService rabbitMqService, ILogger<AccrueInterestHandler> logger)
        {
            _accountsRepository = accountsRepository;
            _context = applicationContext;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task Handle(BlockAccountRequestCommand requestCommand,
            CancellationToken cancellationToken)
        {
            using var transaction =
                _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            try
            {
                var body = JsonSerializer.Deserialize<ClientBlocked>(requestCommand.Message);
                if (body != null &&
                    await _context.Inboxes.FirstOrDefaultAsync(b => b.Payload.Equals(body), cancellationToken: cancellationToken) == null)
                {
                    var meta = body.Meta;
                    var payload = JsonSerializer.Deserialize<ClientBlocked>(requestCommand.Message);
                    if (meta is { Version: "v1" })
                    {
                        if (payload != null)
                        {
                            await _accountsRepository.FrozeAllUserAccounts(payload.Payload.ClientId, cancellationToken);
                        }

                        var inbox = new Inbox(Guid.NewGuid(), GetType().Name, JsonSerializer.Serialize(payload));
                        await _context.Inboxes.AddAsync(inbox, cancellationToken);
                    }
                    else
                    {
                        var inbox = new Inbox(Guid.NewGuid(), GetType().Name, JsonSerializer.Serialize(payload));
                        var deadLetter = new InboxDeadLetters(inbox, "Version not supported");
                        await _context.InboxDeadLetters.AddAsync(deadLetter, cancellationToken);

                        _logger.LogWarning("InboxDeadLetters\n{Serialize}", JsonSerializer.Serialize(body));
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