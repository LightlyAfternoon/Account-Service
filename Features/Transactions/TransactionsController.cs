using Account_Service.Features.Accounts;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Account_Service.Features.Transactions
{
    [Route("transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsService _transactionsService;

        public TransactionsController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }

        [HttpGet]
        public IResult GetAccountStatementOnPeriod(Guid accountId, DateTime startDate, DateTime endDate)
        {
            return Results.Json(_transactionsService.GetAccountStatementOnPeriod(accountId, startDate, endDate));
        }

        [HttpPost]
        public IResult Post([FromBody] TransactionDto transactionDto)
        {
            return Results.Json(_transactionsService.Add(transactionDto));
        }

        [HttpPost("from/{accountIdFrom}/to/{accountIdTo}")]
        public IResult MakeTransfer(Guid fromAccountId, Guid toAccountId, [FromBody] TransactionDto transactionDto)
        {
            return Results.Json(_transactionsService.Transfer(fromAccountId, toAccountId, transactionDto));
        }
    }
}