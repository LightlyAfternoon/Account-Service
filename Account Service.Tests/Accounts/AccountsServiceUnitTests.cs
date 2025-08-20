using Account_Service.Features.Accounts;
using Account_Service.Features.Accounts.AccountsList;
using Account_Service.Features.Accounts.GetAccount;
using Account_Service.Features.Accounts.GetClientCurrentAccountBalance;
using Account_Service.Features.Users;
using Account_Service.Infrastructure.Mappers;
using MediatR;
using Moq;
using Xunit;

namespace Account_Service.Tests.Accounts
    // ReSharper disable once ArrangeNamespaceBody
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountsServiceUnitTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IAccountsRepository> _accountsRepository;
        private readonly IAccountsService _accountsService;

        public AccountsServiceUnitTests()
        {
            _mediator = new Mock<IMediator>();
            _accountsRepository = new Mock<IAccountsRepository>();
            _userRepository = new Mock<IUserRepository>();
            _accountsService = new AccountsService(_mediator.Object);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public async Task FindById()
        {
            var guid = Guid.Parse("C1C2E3F7-C66A-480B-814D-591FC15F9954");
            var requestCommand = new GetAccountByIdRequestCommand(guid);
            var account = new Account(guid, Guid.NewGuid(), AccountType.Checking, CurrencyCode.Rub,
                100.0m, null, DateOnly.FromDateTime(DateTime.Today), null);

            _accountsRepository.Setup(r => r.FindById(guid)).ReturnsAsync(account);
            _mediator.Setup(m => m.Send(requestCommand, CancellationToken.None))
                .Returns(new GetAccountByIdHandler(_accountsRepository.Object).Handle(requestCommand, CancellationToken.None)!);

            Assert.Equal(AccountMappers.MapToDto(account), await _accountsService.FindById(guid));
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public async Task FindAll()
        {
            var requestCommand = new GetAccountsListRequestCommand();
            var account = new Account(Guid.NewGuid(), Guid.NewGuid(), AccountType.Checking, CurrencyCode.Rub,
                100.0m, null, DateOnly.FromDateTime(DateTime.Today), null);

            _accountsRepository.Setup(r => r.FindAll()).ReturnsAsync([account]);
            _mediator.Setup(m => m.Send(requestCommand, CancellationToken.None))
                .Returns(new GetAccountsListHandler(_accountsRepository.Object).Handle(requestCommand, CancellationToken.None));

            Assert.Equal([AccountMappers.MapToDto(account)], await _accountsService.FindAll());

            var account2 = new Account(Guid.NewGuid(), Guid.NewGuid(), AccountType.Credit, CurrencyCode.Rub,
                50.0m, null, DateOnly.FromDateTime(DateTime.Today), null);

            _accountsRepository.Setup(r => r.FindAll()).ReturnsAsync([account, account2]);
            _mediator.Setup(m => m.Send(requestCommand, CancellationToken.None))
                .Returns(new GetAccountsListHandler(_accountsRepository.Object).Handle(requestCommand, CancellationToken.None));

            Assert.Equal([AccountMappers.MapToDto(account), AccountMappers.MapToDto(account2)], await _accountsService.FindAll());
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public async Task GetClientCurrentAccountBalance()
        {
            var guid = Guid.Parse("C1C2E3F7-C66A-480B-814D-591FC15F9954");
            var ownerId = Guid.Parse("27DEE97E-7CEE-4025-882C-B8ACCA11A6A3");
            var requestCommand = new GetClientCurrentAccountBalanceRequestCommand(ownerId);
            var user = new User(ownerId, "new user");
            var account = new Account(guid, ownerId, AccountType.Credit, CurrencyCode.Rub,
                100.0m, null, DateOnly.FromDateTime(DateTime.Today), null);

            _userRepository.Setup(r => r.FindById(ownerId)).ReturnsAsync(user);
            _accountsRepository.Setup(r => r.FindAllByOwnerId(ownerId)).ReturnsAsync([account]);
            _mediator.Setup(m => m.Send(requestCommand, CancellationToken.None))
                .Returns(new GetClientCurrentAccountBalanceHandler(_userRepository.Object, _accountsRepository.Object).Handle(requestCommand, CancellationToken.None)!);

            Assert.Null(await _accountsService.GetClientCurrentAccountBalance(ownerId));

            var guid2 = Guid.Parse("BD0285BF-AC22-49F4-92B7-AA4B5889CEC9");
            var account2 = new Account(guid2, ownerId, AccountType.Checking, CurrencyCode.Rub,
                50.0m, null, DateOnly.FromDateTime(DateTime.Today), null);

            _accountsRepository.Setup(r => r.FindAllByOwnerId(ownerId)).ReturnsAsync([account, account2]);
            _mediator.Setup(m => m.Send(requestCommand, CancellationToken.None))
                .Returns(new GetClientCurrentAccountBalanceHandler(_userRepository.Object, _accountsRepository.Object).Handle(requestCommand, CancellationToken.None)!);

            Assert.Equal(new GetClientCurrentAccountBalanceResponse(guid2, ownerId, account2.Balance), await _accountsService.GetClientCurrentAccountBalance(ownerId));
        }
    }
}