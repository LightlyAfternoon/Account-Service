﻿using Account_Service.Features.Transactions.AddTransaction;
using Account_Service.Features.Transactions.AddTransferTransactions;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Account_Service.Features.Transactions
{
    /// <inheritdoc />
    [Route("transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsService _transactionsService;

        /// <inheritdoc />
        public TransactionsController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }

        /// <summary>
        /// Получение выписки по счёту
        /// </summary>
        /// <param name="accountId">id счёта</param>
        /// <param name="startDate">начальная дата периода</param>
        /// <param name="endDate">конечная дата периода</param>
        /// <returns>Выписку по счёту в указанном периоде</returns>
        [HttpGet]
        public async Task<IResult> GetAccountStatementOnPeriod(Guid accountId, DateTime startDate, DateTime endDate)
        {
            return Results.Json(await _transactionsService.GetAccountStatementOnPeriod(accountId, startDate, endDate));
        }

        /// <summary>
        /// Добавление новой транзакции
        /// </summary>
        /// <param name="requestCommand">Данные транзакции</param>
        /// <returns>Данные транзакции с новым id</returns>
        [HttpPost]
        public async Task<IResult> Post([FromBody] AddTransactionRequestCommand requestCommand)
        {
            return Results.Json(await _transactionsService.Add(requestCommand));
        }

        /// <summary>
        /// Добавление перевода между счетами
        /// </summary>
        /// <param name="fromAccountId">id аккаунта, с которого производится перевод</param>
        /// <param name="toAccountId">id аккаунта, на который производится перевод</param>
        /// <param name="requestCommand">Данные транзакции</param>
        /// <returns>Данные транзакции счёта, с которого произведён перевод</returns>
        [HttpPost("from/{fromAccountId}/to/{toAccountId}")]
        public async Task<IResult> MakeTransfer(Guid fromAccountId, Guid toAccountId, [FromBody] AddTransferTransactionsRequestCommand requestCommand)
        {
            return Results.Json(await _transactionsService.Transfer(fromAccountId, toAccountId, requestCommand));
        }
    }
}