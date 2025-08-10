using Account_Service.Features.Accounts;
using Account_Service.Features.Accounts.GetAccount;
using Account_Service.Infrastructure.Mappers;
using MediatR;
using Moq;
using Xunit;

namespace Account_Service.Tests.Accounts
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountServiceUnitTests
    {
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public async Task FindById()
        {
            var mediator = new Mock<IMediator>();
            IAccountsService accountService = new AccountsService(mediator.Object);
            var accountsRepository = new Mock<IAccountsRepository>();
            Guid guid = Guid.Parse("C1C2E3F7-C66A-480B-814D-591FC15F9954");
            GetAccountByIdRequestCommand requestCommand = new GetAccountByIdRequestCommand(guid);
            Account account = new Account(guid, Guid.NewGuid(), AccountType.Checking, CurrencyCode.Rub,
                100.0m, null, DateOnly.FromDateTime(DateTime.Today), null);

            accountsRepository.Setup(r => r.FindById(guid)).ReturnsAsync(account);
            mediator.Setup(m => m.Send(requestCommand, CancellationToken.None))
                .Returns(new GetAccountByIdHandler(accountsRepository.Object).Handle(requestCommand, CancellationToken.None)!);

            Assert.Equal(AccountMappers.MapToDto(account), await accountService.FindById(guid));
        }
    }
}