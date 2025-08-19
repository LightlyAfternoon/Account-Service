using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Account_Service.Features.Accounts;
using Account_Service.Features.Accounts.AddAccount;
using Account_Service.Features.Accounts.AddAccount.RabbitMQ;
using Account_Service.Features.RabbitMQ;
using Account_Service.Features.Transactions;
using Account_Service.Features.Transactions.AddTransaction;
using Account_Service.Infrastructure;
using Account_Service.Tests.Fixture;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Xunit;

// на данный момент нормально работают только если запускать по одному, в чём причина пока не ясно
namespace Account_Service.Tests.RabbitMQ
    // ReSharper disable once ArrangeNamespaceBody
{
    [Collection("Non-Parallel Tests")]
    public class RabbitMqIntegrationTests : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
            .WithImage("postgres:17.4")
            .WithDatabase("account_service_test_3")
            .WithCleanUp(true)
            .Build();

        private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:4-management")
            .WithUsername("test")
            .WithPassword("test")
            .WithPortBinding(61142, 5672)
            .WithCleanUp(true)
            .Build();

        private HttpClient? _httpClient;

        private IServiceScope _scope = null!;
        private IRabbitMqService _rabbitMqService = null!;
        private IAccountsService _accountsService = null!;

        public async ValueTask InitializeAsync()
        {
            await _container.StartAsync();
            await _rabbitMqContainer.StartAsync();

            Environment.SetEnvironmentVariable("DbSettings__ConnectionString", _container.GetConnectionString());
            Environment.SetEnvironmentVariable("RABBITMQ_DEFAULT_USER", "test");
            Environment.SetEnvironmentVariable("RABBITMQ_DEFAULT_PASS", "test");
            Environment.SetEnvironmentVariable("RABBITMQ_DEFAULT_PORT", _rabbitMqContainer.GetMappedPublicPort(5672).ToString());
            CustomWebApplicationFactory<Program> factory = new(_container.GetConnectionString());

            _scope = factory.Services.CreateScope();
            _rabbitMqService = _scope.ServiceProvider.GetRequiredService<IRabbitMqService>();
            _accountsService = _scope.ServiceProvider.GetRequiredService<IAccountsService>();

            _httpClient = factory.CreateClient();
        }

        public async ValueTask DisposeAsync()
        {
            _httpClient = null;

            await _rabbitMqContainer.StopAsync();
            await _container.StopAsync();
            await _rabbitMqContainer.DisposeAsync();
            await _container.DisposeAsync();
        }

        [Fact]
        public async Task OutboxPublishesAfterFailureTest()
        {
            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken: TestContext.Current.CancellationToken);

            if (_httpClient != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");

                var contentFirstAccount = JsonContent.Create(new AddAccountRequestCommand(
                    ownerId: Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), type: nameof(AccountType.Checking),
                    currency: nameof(CurrencyCode.Eur), balance: decimal.Zero, interestRate: null,
                    openDate: DateOnly.FromDateTime(DateTime.UtcNow), closeDate: null));
                var accountDto = await (await _httpClient.PostAsync("/accounts", contentFirstAccount, TestContext.Current.CancellationToken))
                        .Content
                        .ReadFromJsonAsync<MbResult<AccountDto?>>(TestContext.Current.CancellationToken);

                Assert.NotNull(_rabbitMqService.Connect());

                var channel = await _rabbitMqService.Connect()!.CreateChannelAsync(cancellationToken: TestContext.Current.CancellationToken);

                await channel.ExchangeDeclareAsync("account.events", ExchangeType.Topic, cancellationToken: TestContext.Current.CancellationToken);

                await channel.QueueDeclareAsync("account.crm", cancellationToken: TestContext.Current.CancellationToken);
                await channel.QueueBindAsync(queue: "account.crm", exchange: "account.events", routingKey: "account.*", cancellationToken: TestContext.Current.CancellationToken);

                var consumer = new AsyncEventingBasicConsumer(channel);
                var receivedMessages = new List<string>();
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    receivedMessages.Add(message);
                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                };

                await channel.BasicConsumeAsync(queue: "account.crm",
                    autoAck: false,
                    consumer: consumer,
                    cancellationToken: TestContext.Current.CancellationToken);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken: TestContext.Current.CancellationToken);

                Assert.Single(receivedMessages.Select(m =>
                    JsonSerializer.Deserialize<AccountOpened>(m)!).Where(m =>
                    m.Payload.OwnerId.Equals(accountDto!.Value!.OwnerId) &&
                    m.Payload.Type.Equals(accountDto.Value!.Type) &&
                    m.Payload.Currency.Equals(accountDto.Value!.Currency)).ToList());

                await _rabbitMqContainer.StopAsync(TestContext.Current.CancellationToken);

                var contentSecondAccount = JsonContent.Create(new AddAccountRequestCommand(
                    ownerId: Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), type: nameof(AccountType.Deposit),
                    currency: nameof(CurrencyCode.Usd), balance: decimal.Zero, interestRate: 0.1m,
                    openDate: DateOnly.FromDateTime(DateTime.UtcNow), closeDate: null));
                var accountDto2 = await (await _httpClient.PostAsync("/accounts", contentSecondAccount, TestContext.Current.CancellationToken))
                    .Content
                    .ReadFromJsonAsync<MbResult<AccountDto?>>(TestContext.Current.CancellationToken);

                await _rabbitMqContainer.StartAsync(TestContext.Current.CancellationToken);

                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken: TestContext.Current.CancellationToken);

                Assert.Single(receivedMessages.Select(m =>
                    JsonSerializer.Deserialize<AccountOpened>(m)!).Where(m =>
                    m.Payload.OwnerId.Equals(accountDto!.Value!.OwnerId) &&
                    m.Payload.Type.Equals(accountDto.Value!.Type) &&
                    m.Payload.Currency.Equals(accountDto.Value!.Currency)).ToList());

                await _rabbitMqService.PublishAllNonProcessedFromOutbox();

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken: TestContext.Current.CancellationToken);

                Assert.Single(receivedMessages.Select(m =>
                    JsonSerializer.Deserialize<AccountOpened>(m)!).Where(m =>
                    m.Payload.OwnerId.Equals(accountDto!.Value!.OwnerId) &&
                    m.Payload.Type.Equals(accountDto.Value!.Type) &&
                    m.Payload.Currency.Equals(accountDto.Value!.Currency)).ToList());
                Assert.Single(receivedMessages.Select(m =>
                    JsonSerializer.Deserialize<AccountOpened>(m)!).Where(m =>
                    m.Payload.OwnerId.Equals(accountDto2!.Value!.OwnerId) &&
                    m.Payload.Type.Equals(accountDto2.Value!.Type) &&
                    m.Payload.Currency.Equals(accountDto2.Value!.Currency)).ToList());

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken: TestContext.Current.CancellationToken);

                await _rabbitMqService.PublishAllNonProcessedFromOutbox();

                Assert.Single(receivedMessages.Select(m =>
                    JsonSerializer.Deserialize<AccountOpened>(m)!).Where(m =>
                    m.Payload.OwnerId.Equals(accountDto!.Value!.OwnerId) &&
                    m.Payload.Type.Equals(accountDto.Value!.Type) &&
                    m.Payload.Currency.Equals(accountDto.Value!.Currency)).ToList());
                Assert.Single(receivedMessages.Select(m =>
                        JsonSerializer.Deserialize<AccountOpened>(m)!).Where(m =>
                    m.Payload.OwnerId.Equals(accountDto2!.Value!.OwnerId) &&
                    m.Payload.Type.Equals(accountDto2.Value!.Type) &&
                    m.Payload.Currency.Equals(accountDto2.Value!.Currency)).ToList());
            }
        }

        [Fact]
        public async Task ClientBlockedPreventsDebitTest()
        {
            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken: TestContext.Current.CancellationToken);

            if (_httpClient != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");

                var contentFirstAccount = JsonContent.Create(new AddAccountRequestCommand(
                    ownerId: Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), type: nameof(AccountType.Checking),
                    currency: nameof(CurrencyCode.Usd), balance: 5m, interestRate: null,
                    openDate: DateOnly.FromDateTime(DateTime.UtcNow), closeDate: null));
                var accountDto = await (await _httpClient.PostAsync("/accounts", contentFirstAccount, TestContext.Current.CancellationToken))
                        .Content
                        .ReadFromJsonAsync<MbResult<AccountDto?>>(TestContext.Current.CancellationToken);

                Assert.NotNull(_rabbitMqService.Connect());

                await _rabbitMqService.PublishClientBlocked(accountDto!.Value!.OwnerId, TestContext.Current.CancellationToken);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken: TestContext.Current.CancellationToken);

                Assert.True((await _accountsService.FindById(accountDto.Value.Id))!.Frozen);

                var contentCreditTransaction = JsonContent.Create(new AddTransactionRequestCommand(
                    accountId: accountDto.Value.Id, sum: 129m, currency: nameof(CurrencyCode.Rub),
                    type: nameof(TransactionType.Credit), description: "first credit", dateTime: DateTime.UtcNow));
                var creditTransactionDto = await (await _httpClient.PostAsync("/transactions", contentCreditTransaction, TestContext.Current.CancellationToken))
                    .Content
                    .ReadFromJsonAsync<MbResult<TransactionDto?>>(TestContext.Current.CancellationToken);

                Assert.Equal(new MbResult<TransactionDto?>(status: HttpStatusCode.Created)
                    { Value = new TransactionDto((Guid)creditTransactionDto?.Value?.Id!, creditTransactionDto.Value) }, creditTransactionDto);

                var contentDebitTransaction = JsonContent.Create(new AddTransactionRequestCommand(
                    accountId: accountDto.Value.Id, sum: 44m, currency: nameof(CurrencyCode.Rub),
                    type: nameof(TransactionType.Debit), description: "first debit", dateTime: DateTime.UtcNow));
                var debitTransactionDto = await (await _httpClient.PostAsync("/transactions", contentDebitTransaction, TestContext.Current.CancellationToken))
                    .Content
                    .ReadFromJsonAsync<MbResult<TransactionDto?>>(TestContext.Current.CancellationToken);

                Assert.Equal(new MbResult<TransactionDto?>(status: HttpStatusCode.Conflict)
                    { MbError = ["С замороженного счёта нельзя снимать деньги"] }, debitTransactionDto);
            }
        }
    }
}
