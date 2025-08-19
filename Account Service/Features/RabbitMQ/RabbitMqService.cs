using System.Text;
using Account_Service.Features.Accounts;
using Account_Service.Features.Accounts.AccrueInterest;
using Account_Service.Features.Accounts.Antifraud.BlockAccount.RabbitMQ;
using Account_Service.Infrastructure.Db;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Account_Service.Features.RabbitMQ
// ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public class RabbitMqService : IRabbitMqService
    {
        private static IConnection? _connection;
        private readonly ConnectionFactory _factory;
        private readonly List<AmqpTcpEndpoint> _endpoints;
        private static IChannel? _producerChannel;
        private static IChannel? _consumerChannel;
        private readonly ILogger<RabbitMqService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionFactory"></param>
        /// <param name="endpoints"></param>
        /// <param name="logger"></param>
        /// <param name="serviceScopeFactory"></param>
        public RabbitMqService(ConnectionFactory connectionFactory, List<AmqpTcpEndpoint> endpoints,
            ILogger<RabbitMqService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _factory = connectionFactory;
            _endpoints = endpoints;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IConnection? Connect()
        {
            if (_connection != null)
            {
                return _connection;
            }

            try
            {
                _connection = _factory.CreateConnectionAsync(_endpoints).Result;
                _producerChannel = GetProducerChannel();
                _consumerChannel = GetConsumerChannel();
                Consume().RunSynchronously();
                return _connection;
            }
            catch
            {
                return null;
            }
        }

        private static IChannel? GetProducerChannel()
        {
            if (_connection is not { IsOpen: true })
                return null;

            if (_connection is { IsOpen: true } && _producerChannel != null)
            {
                return _producerChannel;
            }

            try
            {
                var channel = _connection.CreateChannelAsync().Result;

                channel.ExchangeDeclareAsync("account.events", ExchangeType.Topic);

                channel.QueueDeclareAsync("account.crm");
                channel.QueueBindAsync(queue: "account.crm", exchange: "account.events", routingKey: "account.*");

                channel.QueueDeclareAsync("account.notifications");
                channel.QueueBindAsync(queue: "account.notifications", exchange: "account.events",
                    routingKey: "money.*");

                channel.QueueDeclareAsync("account.antifraud");
                channel.QueueBindAsync(queue: "account.antifraud", exchange: "account.events",
                    routingKey: "client.#");

                channel.QueueDeclareAsync("account.audit");
                channel.QueueBindAsync(queue: "account.audit", exchange: "account.events", routingKey: "#");

                return channel;
            }
            catch
            {
                return null;
            }
        }

        private static IChannel? GetConsumerChannel()
        {
            if (_connection is not { IsOpen: true })
                return null;

            if (_connection is { IsOpen: true } && _consumerChannel != null)
            {
                return _consumerChannel;
            }

            try
            {
                var channel = _connection.CreateChannelAsync().Result;

                channel.ExchangeDeclareAsync("account.events", ExchangeType.Topic);

                channel.QueueDeclareAsync("account.antifraud");
                channel.QueueBindAsync(queue: "account.antifraud", exchange: "account.events",
                    routingKey: "client.#");

                return channel;
            }
            catch
            {
                return null;
            }
        }

        /// <inheritdoc />
        public async Task Publish(Outbox outbox, CancellationToken cancellationToken)
        {
            if (_connection == null)
            {
                Connect();
            }

            if (_producerChannel != null && !outbox.ProcessedAt.HasValue)
            {
                try
                {
                    var bodyBytes = Encoding.UTF8.GetBytes(outbox.Payload);
                    await _producerChannel.BasicPublishAsync(exchange: "account.events",
                        routingKey: outbox.RoutingKey, body: bodyBytes, cancellationToken: cancellationToken);

                    var body = JsonConvert.DeserializeObject<dynamic>(outbox.Payload)!;
                    _logger.LogInformation((string)"Publish: {EventId}, {Meta}", (object?)body.EventId, (object?)body.Meta);

                    outbox.ProcessedAt = DateTime.UtcNow;

                    using var scope = _serviceScopeFactory.CreateScope();
                    var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();

                    await outboxRepository.Save(outbox, cancellationToken);
                }
                catch (AlreadyClosedException)
                {
                    _logger.LogError("Connection to RabbitMQ is closed, unable to publish");
                }
            }
        }

        /// <inheritdoc />
        public async Task PublishAllNonProcessedFromOutbox()
        {
            if (_producerChannel != null)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();

                foreach (var outbox in (await outboxRepository.FindAllNotProcessed()).Where(outbox => !outbox.ProcessedAt.HasValue))
                {
                    await Publish(outbox, CancellationToken.None);
                }

                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                await context.SaveChangesAsync(CancellationToken.None);
            }
        }

        /// <inheritdoc />
        public async Task PublishClientBlocked(Guid id, CancellationToken cancellationToken)
        {
            if (_connection == null)
            {
                Connect();
            }

            if (_producerChannel != null)
            {
                try
                {
                    Outbox outbox = new(Guid.Empty, "client.blocked", nameof(AccrueInterestHandler),
                        JsonSerializer.Serialize(new ClientBlocked(Guid.NewGuid(), DateTime.UtcNow,
                            new ClientBlockedPayload(id), new Meta(version: "v1", source: "Client Service",
                                correlationId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                                causationId: Guid.Parse("22222222-2222-2222-2222-222222222222")))));
                    var bodyBytes = Encoding.UTF8.GetBytes(outbox.Payload);
                    await _producerChannel.BasicPublishAsync(exchange: "account.events",
                        routingKey: outbox.RoutingKey, body: bodyBytes, cancellationToken: cancellationToken);

                    var body = JsonConvert.DeserializeObject<dynamic>(outbox.Payload)!;
                    _logger.LogInformation((string)"Publish: {EventId}, {Meta}", (object?)body.EventId, (object?)body.Meta);
                }
                catch (AlreadyClosedException)
                {
                    _logger.LogError("Connection to RabbitMQ is closed, unable to publish");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task Consume()
        {
            if (_connection == null)
            {
                Connect();
            }

            if (_consumerChannel != null)
            {
                try
                {
                    var antifraudConsumer = new AsyncEventingBasicConsumer(_consumerChannel);

                    antifraudConsumer.ReceivedAsync += async (_, ea) =>
                    {
                        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                        switch (ea.RoutingKey)
                        {
                            case "client.blocked":
                            {
                                using var scope = _serviceScopeFactory.CreateScope();
                                var service = scope.ServiceProvider.GetRequiredService<IAccountsService>();

                                await service.BlockAccount(message);
                                break;
                            }
                            case "client.unblocked":
                            {
                                using var scope = _serviceScopeFactory.CreateScope();
                                var service = scope.ServiceProvider.GetRequiredService<IAccountsService>();

                                await service.UnblockAccount(message);
                                break;
                            }
                        }

                        await _consumerChannel.BasicAckAsync(ea.DeliveryTag, false);

                        _logger.LogInformation("Consume: {Message}", message);
                    };

                    await _consumerChannel.BasicQosAsync(0, 1, false);
                    await _consumerChannel.BasicConsumeAsync("account.antifraud", false, antifraudConsumer);
                }
                catch (AlreadyClosedException)
                {
                    _logger.LogError("Connection to RabbitMQ is closed, unable to consume");
                }
            }
        }
    }
}