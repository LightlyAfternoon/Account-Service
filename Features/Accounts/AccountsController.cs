using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Account_Service.Features.Accounts
{
    [Route("accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IResult GetAllAccounts()
        {
            return Results.Json(_accountService.FindAll());
        }

        [HttpGet("{id}")]
        public IResult GetClientHasAnyAccount(Guid id, bool hasAnyAccount)
        {
            if (hasAnyAccount)
            {
                if (_accountService.ClientWithIdHasAnyAccount(id))
                    return Results.Ok("Yes");
                else
                    return Results.Ok("No");
            }
            else
            {
                if (_accountService.ClientWithIdHasAnyAccount(id))
                    return Results.Ok("No");
                else
                    return Results.Ok("Yes");
            }
        }

        [HttpPost]
        public IResult Post([FromBody] AccountDto accountDto)
        {
            return Results.Json(_accountService.Add(accountDto));
        }

        [HttpPut("{id}")]
        public IResult Put(Guid id, [FromBody] AccountDto accountDto)
        {
            return Results.Json(_accountService.Update(id, accountDto));
        }

        [HttpDelete("{id}")]
        public IResult Delete(Guid id)
        {
            return Results.Json(_accountService.DeleteById(id));
        }
    }
}