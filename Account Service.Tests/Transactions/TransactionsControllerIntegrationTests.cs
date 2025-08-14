using Account_Service.Features.Accounts;
using Account_Service.Features.Accounts.AddAccount;
using Account_Service.Features.Accounts.GetClientCurrentAccountBalance;
using Account_Service.Features.Transactions;
using Account_Service.Features.Transactions.AddTransaction;
using Account_Service.Features.Transactions.AddTransferTransactions;
using Account_Service.Infrastructure;
using Account_Service.Tests.Fixture;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Account_Service.Infrastructure.Db;
using Testcontainers.PostgreSql;
using Xunit;

namespace Account_Service.Tests.Transactions
{
    public class TransactionsControllerIntegrationTests : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
            .WithImage("postgres:17.4")
            .WithCleanUp(true)
            .Build();

        private HttpClient? _httpClient;

        private  IServiceScope _scope = null!;
        private  ApplicationContext _context = null!;

        public async ValueTask InitializeAsync()
        {
            await _container.StartAsync();

            Environment.SetEnvironmentVariable("DbSettings__ConnectionString", _container.GetConnectionString());
            CustomWebApplicationFactory<Program> factory = new(_container.GetConnectionString());

            _scope = factory.Services.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            _httpClient = factory.CreateClient();
        }

        public async ValueTask DisposeAsync()
        {
            _httpClient = null;

            await new ValueTask(_container.StartAsync());
        }

        [Fact]
        public async Task ParallelTransferTest()
        {
            if (_httpClient != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");

                JsonContent contentFrom = JsonContent.Create(new AddAccountRequestCommand(
                    ownerId: Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), type: nameof(AccountType.Checking),
                    currency: nameof(CurrencyCode.Rub), balance: Decimal.Zero, interestRate: null,
                    openDate: DateOnly.FromDateTime(DateTime.UtcNow), closeDate: null));
                var accountFrom =
                    await (await _httpClient.PostAsync("/accounts", contentFrom, TestContext.Current.CancellationToken))
                        .Content
                        .ReadFromJsonAsync<MbResult<AccountDto?>>(TestContext.Current.CancellationToken);

                JsonContent contentTo = JsonContent.Create(new AddAccountRequestCommand(
                    ownerId: Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), type: nameof(AccountType.Deposit),
                    currency: nameof(CurrencyCode.Rub), balance: Decimal.Zero, interestRate: null,
                    openDate: DateOnly.FromDateTime(DateTime.UtcNow), closeDate: null));
                var accountTo =
                    await (await _httpClient.PostAsync("/accounts", contentTo, TestContext.Current.CancellationToken)).Content
                        .ReadFromJsonAsync<MbResult<AccountDto?>>(TestContext.Current.CancellationToken);

                Assert.Equal(new MbResult<AccountDto?>(status: HttpStatusCode.Created)
                        { Value = new AccountDto((Guid)accountFrom?.Value?.Id!, accountFrom.Value) }, accountFrom);
                Assert.Equal(new MbResult<AccountDto?>(status: HttpStatusCode.Created)
                        { Value = new AccountDto((Guid)accountTo?.Value?.Id!, accountTo.Value) }, accountTo);

                JsonContent contentFirstTransaction = JsonContent.Create(new AddTransactionRequestCommand(
                    accountFrom.Value.Id, 1_000m, nameof(CurrencyCode.Rub), nameof(TransactionType.Debit),
                    "первая транзакция", DateTime.UtcNow));
                var firstTransaction =
                    await (await _httpClient.PostAsync("/transactions", contentFirstTransaction, TestContext.Current.CancellationToken)).Content
                        .ReadFromJsonAsync<MbResult<TransactionDto?>>(TestContext.Current.CancellationToken);

                Assert.Equal(new MbResult<TransactionDto?>(status: HttpStatusCode.Created)
                    { Value = new TransactionDto(firstTransaction!.Value!.Id, firstTransaction.Value) }, firstTransaction);
                Assert.Equal(new MbResult<GetClientCurrentAccountBalanceResponse>(status: HttpStatusCode.OK)
                    { Value = new GetClientCurrentAccountBalanceResponse(accountFrom.Value.Id, Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), 1_000m) },
                    await (await _httpClient.GetAsync("accounts/client/3fa85f64-5717-4562-b3fc-2c963f66afa6/current-account-balance",
                            TestContext.Current.CancellationToken)).Content
                        .ReadFromJsonAsync<MbResult<GetClientCurrentAccountBalanceResponse>>(TestContext.Current.CancellationToken));

                var requestCommand = new AddTransferTransactionsRequestCommand(accountFrom.Value.Id,
                    accountTo.Value.Id, 370m, nameof(CurrencyCode.Rub), "перевод", DateTime.UtcNow);
                JsonContent contentTransfer = JsonContent.Create(requestCommand);

                List<Task<HttpResponseMessage>> tasks = new List<Task<HttpResponseMessage>>(50);
                for (int i = 0; i < 50; i++)
                {
                    tasks.Add(_httpClient.PostAsync(
                        $"/transactions/from/{accountFrom.Value.Id}/to/{accountTo.Value.Id}", contentTransfer,
                        TestContext.Current.CancellationToken));
                }
                var all = await Task.WhenAll(tasks);
                int countSuccessful = all.Count(t => t.IsSuccessStatusCode);

#pragma warning disable xUnit2012
                Assert.True(all.Any(t => t.IsSuccessStatusCode));
#pragma warning restore xUnit2012
#pragma warning disable xUnit2012
                Assert.True(all.Any(t => t.StatusCode.Equals(HttpStatusCode.Conflict)));
#pragma warning restore xUnit2012

                Assert.Equal(new MbResult<GetClientCurrentAccountBalanceResponse>(status: HttpStatusCode.OK)
                { Value = new GetClientCurrentAccountBalanceResponse(accountFrom.Value.Id, Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), 1000m - 370 * countSuccessful) },
                    await (await _httpClient.GetAsync("accounts/client/3fa85f64-5717-4562-b3fc-2c963f66afa6/current-account-balance",
                            TestContext.Current.CancellationToken)).Content
                        .ReadFromJsonAsync<MbResult<GetClientCurrentAccountBalanceResponse>>(TestContext.Current.CancellationToken));
#pragma warning disable xUnit1051
                Assert.Equal(370m * countSuccessful, ((await _context.Accounts.FindAsync(accountTo.Value.Id, TestContext.Current.CancellationToken))!).Balance);
#pragma warning restore xUnit1051
                Assert.Equal(1000m,
#pragma warning disable xUnit1051
                    ((await _context.Accounts.FindAsync(accountFrom.Value.Id, TestContext.Current.CancellationToken))!).Balance +
#pragma warning restore xUnit1051
#pragma warning disable xUnit1051
                    ((await _context.Accounts.FindAsync(accountTo.Value.Id, TestContext.Current.CancellationToken))!).Balance);
#pragma warning restore xUnit1051
            }
        }
    }
}