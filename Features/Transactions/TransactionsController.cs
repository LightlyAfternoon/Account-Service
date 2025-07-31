using Account_Service.Features.Transactions.AddTransaction;
using Account_Service.Features.Transactions.AddTransferTransactions;
using Account_Service.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        /// <param name="startDate">Начальная дата периода</param>
        /// <param name="endDate">Конечная дата периода</param>
        /// <returns>Выписку по счёту в указанном периоде</returns>
        [HttpGet]
        public async Task<MbResult<List<TransactionDto>>> GetAccountStatementOnPeriod(Guid accountId, DateTime startDate, DateTime endDate)
        {
            return new MbResult<List<TransactionDto>> { Status = HttpStatusCode.OK, Value = await _transactionsService.GetAccountStatementOnPeriod(accountId, startDate, endDate) };
        }

        /// <summary>
        /// Добавление новой транзакции
        /// </summary>
        /// <param name="requestCommand">Данные транзакции</param>
        /// <returns>Данные транзакции с новым id</returns>
        [HttpPost]
        public async Task<MbResult<TransactionDto?>> Post([FromBody] AddTransactionRequestCommand requestCommand)
        {
            return new MbResult<TransactionDto?> { Status = HttpStatusCode.OK, Value = await _transactionsService.Add(requestCommand) };
        }

        /// <summary>
        /// Добавление перевода между счетами
        /// </summary>
        /// <param name="fromAccountId">Id аккаунта, с которого производится перевод</param>
        /// <param name="toAccountId">Id аккаунта, на который производится перевод</param>
        /// <param name="requestCommand">Данные транзакции</param>
        /// <returns>Данные транзакции счёта, с которого произведён перевод</returns>
        [HttpPost("from/{fromAccountId}/to/{toAccountId}")]
        public async Task<MbResult<TransactionDto?>> MakeTransfer(Guid fromAccountId, Guid toAccountId, [FromBody] AddTransferTransactionsRequestCommand requestCommand)
        {
            return new MbResult<TransactionDto?> { Status = HttpStatusCode.OK, Value = await _transactionsService.Transfer(fromAccountId, toAccountId, requestCommand) };
        }
    }
}