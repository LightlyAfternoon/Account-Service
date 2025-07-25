﻿using Account_Service.Infrastructure;
using MediatR;

namespace Account_Service.Features.Accounts.AccountsList
{
    /// <inheritdoc />
    public class GetAccountsListHandler : IRequestHandler<GetAccountsListRequestCommand, List<AccountDto>>
    {
        private readonly IAccountsRepository _accountsRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountsRepository"></param>
        public GetAccountsListHandler(IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        /// <inheritdoc />
        public async Task<List<AccountDto>> Handle(GetAccountsListRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            return (await _accountsRepository.FindAll()).Select(AccountMappers.MapToDto).ToList();
        }
    }
}