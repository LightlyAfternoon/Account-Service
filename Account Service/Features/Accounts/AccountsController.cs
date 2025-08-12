using Account_Service.Features.Accounts.AddAccount;
using Account_Service.Features.Accounts.UpdateAccount;
using Account_Service.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Account_Service.Features.Accounts.GetClientCurrentAccountBalance;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Account_Service.Features.Accounts
{
    /// <inheritdoc />
    [Authorize]
    [Route("accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService _accountService;

        /// <inheritdoc />
        public AccountsController(IAccountsService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Получение списка всех счетов
        /// </summary>
        /// <response code="200">MbResult &lt; List &lt; AccountDto &gt; &gt; со Всеми счетами из БД</response>
        [HttpGet]
        public async Task<MbResult<List<AccountDto>>> GetAllAccounts()
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            return new MbResult<List<AccountDto>>(status: HttpStatusCode.OK)
                { Value = await _accountService.FindAll() };
        }

        /// <summary>
        /// Получение информации о том, есть ли у клиента счета
        /// </summary>
        /// <param name="ownerId">Id клиента</param>
        /// <response code="200">MbResult &lt; string &gt; с Сообщением с информации о том, есть или нет у данного клиента счетов</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet("client/{ownerId}/has-any-accounts")]
        public async Task<MbResult<string>> GetClientHasAnyAccount(Guid ownerId)
        {
            if (await _accountService.ClientWithIdHasAnyAccount(ownerId))
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                return new MbResult<string>(status: HttpStatusCode.OK)
                    { Value = "У данного пользователя есть счета" };
            }
            else
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                return new MbResult<string>(status: HttpStatusCode.OK)
                    { Value = "У данного пользователя нет счётов" };
            }
        }

        /// <summary>
        /// Получение баланса текущего счёта клиента
        /// </summary>
        /// <param name="ownerId">Id клиента</param>
        /// <response code="200">MbResult &lt; GetClientCurrentAccountBalanceResponse &gt; с Id текущего счёта, id клиента, баланс счёта</response>
        /// <response code="404">Клиент с данным id не найден или у него нет текущего счёта</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet("client/{ownerId}/current-account-balance")]
        public async Task<MbResult<GetClientCurrentAccountBalanceResponse>> GetClientCurrentAccountBalance(Guid ownerId)
        {
            var response = await _accountService.GetClientCurrentAccountBalance(ownerId);

            if (response != null)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                return new MbResult<GetClientCurrentAccountBalanceResponse>(status: HttpStatusCode.OK)
               { Value = response };
            }
            else
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return new MbResult<GetClientCurrentAccountBalanceResponse>(status: HttpStatusCode.NotFound)
                { MbError = ["Клиент с данным id не найден или у него нет текущего счёта"] };
            }
        }

        /// <summary>
        /// Добавление нового счёта
        /// </summary>
        /// <param name="requestCommand">Данные нового счёта</param>
        /// <response code="200">MbResult &lt; AccountDto? &gt; с Данными счёта с новым id</response>
        /// <response code="400">Ошибка валидации тела запроса</response>
        /// <response code="401">Ошибка валидации токена при аутентификации</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost]
        public async Task<MbResult<AccountDto?>> Post([FromBody] AddAccountRequestCommand requestCommand)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
            return new MbResult<AccountDto?>(status: HttpStatusCode.Created)
                { Value = await _accountService.Add(requestCommand) };
        }

        /// <summary>
        /// Изменение данных счёта
        /// </summary>
        /// <param name="id">id счёта</param>
        /// <param name="requestCommand"> Данные счёта</param>
        /// <response code="200">MbResult &lt; AccountDto? &gt; с Данными изменённого счёта</response>
        /// <response code="400">Ошибка валидации тела запроса</response>
        /// <response code="401">Ошибка валидации токена при аутентификации</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPut("{id}")]
        public async Task<MbResult<AccountDto?>> Put(Guid id, [FromBody] UpdateAccountRequestCommand requestCommand)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            return new MbResult<AccountDto?>(status: HttpStatusCode.OK)
                { Value = await _accountService.Update(id, requestCommand) };
        }

        /// <summary>
        /// Удаление счёта
        /// </summary>
        /// <param name="id">id счёта</param>
        /// <response code="200">MbResult &lt; string &gt; с Сообщением о том, удалён ли счёт или нет</response>
        /// <response code="401">Ошибка валидации токена при аутентификации</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpDelete("{id}")]
        public async Task<MbResult<string>> Delete(Guid id)
        {
            bool isDeleted = await _accountService.DeleteById(id);

            if (isDeleted)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                return new MbResult<string> (status: HttpStatusCode.OK)
                    { Value = $"Пользователь с id={id} удалён" };
            }
            else
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new MbResult<string> (status: HttpStatusCode.BadRequest)
                    { MbError = [$"Не получилось удалить пользователя с id={id}"] };
            }
        }
    }
}