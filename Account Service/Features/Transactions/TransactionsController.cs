using Account_Service.Features.Transactions.AddTransaction;
using Account_Service.Features.Transactions.AddTransferTransactions;
using Account_Service.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Account_Service.Features.Transactions
{
    /// <inheritdoc />
    [Authorize]
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
        /// <response code="200">MbResult &lt; List &lt; TransactionDto &gt; &gt; с Выпиской по счёту в указанном периоде</response>
        /// <response code="401">Ошибка валидации токена при аутентификации</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet]
        public async Task<MbResult<List<TransactionDto>>> GetAccountStatementOnPeriod(Guid accountId, DateTime startDate, DateTime endDate)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            return new MbResult<List<TransactionDto>>(status: HttpStatusCode.OK)
                { Value = await _transactionsService.GetAccountStatementOnPeriod(accountId, startDate, endDate) };
        }

        /// <summary>
        /// Добавление новой транзакции
        /// </summary>
        /// <param name="requestCommand">Данные транзакции</param>
        /// <response code="200">MbResult &lt; TransactionDto? &gt; с Данными транзакции с новым id</response>
        /// <response code="400">Ошибка валидации тела запроса</response>
        /// <response code="401">Ошибка валидации токена при аутентификации</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost]
        public async Task<MbResult<TransactionDto?>> Post([FromBody] AddTransactionRequestCommand requestCommand)
        {
            TransactionDto? transactionDto = await _transactionsService.Add(requestCommand);

            if (transactionDto != null)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
                return new MbResult<TransactionDto?>(status: HttpStatusCode.Created)
                    { Value = transactionDto };
            }
            else
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new MbResult<TransactionDto?>(status: HttpStatusCode.BadRequest)
                    { MbError = ["Не получилось добавить новую транзакцию"] };
            }
        }

        /// <summary>
        /// Добавление перевода между счетами
        /// </summary>
        /// <param name="fromAccountId">Id аккаунта, с которого производится перевод</param>
        /// <param name="toAccountId">Id аккаунта, на который производится перевод</param>
        /// <param name="requestCommand">Данные транзакции</param>
        /// <response code="200">MbResult &lt; TransactionDto? &gt; с Данными транзакции счёта, с которого произведён перевод</response>
        /// <response code="400">Ошибка валидации тела запроса</response>
        /// <response code="401">Ошибка валидации токена при аутентификации</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("from/{fromAccountId}/to/{toAccountId}")]
        public async Task<MbResult<TransactionDto?>> MakeTransfer(Guid fromAccountId, Guid toAccountId, [FromBody] AddTransferTransactionsRequestCommand requestCommand)
        {
            TransactionDto? transactionDto = await _transactionsService.Transfer(fromAccountId, toAccountId, requestCommand);

            if (transactionDto != null)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
                return new MbResult<TransactionDto?>(status: HttpStatusCode.Created)
                    { Value = transactionDto };
            }
            else
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new MbResult<TransactionDto?>(status: HttpStatusCode.BadRequest)
                    { MbError = ["Не получилось совершить перевод"] };
            }
        }
    }
}