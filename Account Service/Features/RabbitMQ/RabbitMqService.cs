using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Account_Service.Features.Accounts.Antifraud.BlockAccount;
using Account_Service.Features.Accounts.Antifraud.UnblockAccount;
using MediatR;

namespace Account_Service.Features.RabbitMQ
{
    /// <inheritdoc />
    public class RabbitMqService : IRabbitMqService
    {
        private static IConnection? _connection;
        private readonly ConnectionFactory _factory;
        private readonly List<AmqpTcpEndpoint> _endpoints;
        private static IChannel? _producerChannel;
        private static IChannel? _consumerChannel;
        private readonly IOutboxRepository _outboxRepository;
        private readonly IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionFactory"></param>
        /// <param name="endpoints"></param>
        /// <param name="outboxRepository"></param>
        /// <param name="mediator"></param>
        public RabbitMqService(ConnectionFactory connectionFactory, List<AmqpTcpEndpoint> endpoints,
            IOutboxRepository outboxRepository, IMediator mediator)
        {
            _factory = connectionFactory;
            _endpoints = endpoints;
            _outboxRepository = outboxRepository;
            _mediator = mediator;
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
                return _connection;
            }
            catch
            {
                _producerChannel = null;
                return null;
            }
        }

        private IChannel? GetProducerChannel()
        {
            if (_producerChannel != null)
            {
                return _producerChannel;
            }

            if (_connection is { IsOpen: true })
            {
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
                        routingKey: "client.*");

                    channel.QueueDeclareAsync("account.audit");
                    channel.QueueBindAsync(queue: "account.audit", exchange: "account.events", routingKey: "#");

                    return channel;
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        private IChannel? GetConsumerChannel()
        {
            if (_consumerChannel != null)
            {
                return _consumerChannel;
            }

            if (_connection is { IsOpen: true })
            {
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

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outbox"></param>
        public async Task Publish(Outbox outbox)
        {
            if (_producerChannel != null && !outbox.ProcessedAt.HasValue)
            {
                var body = Encoding.UTF8.GetBytes(outbox.Payload);
                await _producerChannel.BasicPublishAsync(exchange: "account.events",
                    routingKey: outbox.RoutingKey, body: body);

                outbox.ProcessedAt = DateTime.Now;

                await _outboxRepository.Save(outbox, CancellationToken.None);
            }
        }

        /// <inheritdoc />
        public async Task PublishAllNonProcessedFromOutbox()
        {
            if (_producerChannel != null)
            {
                foreach (var outbox in (await _outboxRepository.FindAll()).Where(b => !b.ProcessedAt.HasValue).ToList())
                {
                    if (!outbox.ProcessedAt.HasValue)
                    {
                        await Publish(outbox);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task Consume()
        {
            if (_consumerChannel != null)
            {
                var antifraudConsumer = new AsyncEventingBasicConsumer(_consumerChannel);

                antifraudConsumer.ReceivedAsync += async (_, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    if (ea.RoutingKey.Equals("antifraud.client.blocked"))
                    {
                        await _mediator.Send(new BlockAccountRequestCommand(message));
                    }
                    else if (ea.RoutingKey.Equals("antifraud.client.unblocked"))
                    {
                        await _mediator.Send(new UnblockAccountRequestCommand(message));
                    }

                    await _consumerChannel.BasicAckAsync(ea.DeliveryTag, false);
                };

                await _consumerChannel.BasicQosAsync(0, 1, false);
                await _consumerChannel.BasicConsumeAsync("account.antifraud", false, antifraudConsumer);
            }
        }
    }
}