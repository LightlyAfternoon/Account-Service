using Account_Service.Features.Accounts.AddAccount;
using Account_Service.Features.Accounts.UpdateAccount;
using Account_Service.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
        private readonly IAccountService _accountService;

        /// <inheritdoc />
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Получение списка всех счетов
        /// </summary>
        /// <returns>Все счета из БД</returns>
        [HttpGet]
        public async Task<MbResult<List<AccountDto>>> GetAllAccounts()
        {
            return new MbResult<List<AccountDto>> { Status = HttpStatusCode.OK, Value = await _accountService.FindAll() };
        }

        /// <summary>
        /// Получение информации о том, есть ли у клиента счета
        /// </summary>
        /// <param name="ownerId">Id клиента</param>
        /// <returns>Сообщение с информации о том, есть или нет у данного клиента счетов</returns>
        [HttpGet("client/{ownerId}")]
        public async Task<MbResult<string>> GetClientHasAnyAccount(Guid ownerId)
        {
            if (await _accountService.ClientWithIdHasAnyAccount(ownerId))
                return new MbResult<string> { Status = HttpStatusCode.OK, Value = "У данного пользователя есть счета" };
            else
                return new MbResult<string> { Status = HttpStatusCode.OK, Value = "У данного пользователя нет счётов" };
        }

        /// <summary>
        /// Добавление нового счёта
        /// </summary>
        /// <param name="requestCommand">Данные нового счёта</param>
        /// <returns>Данные счёта с новым id</returns>
        [HttpPost]
        public async Task<MbResult<AccountDto?>> Post([FromBody] AddAccountRequestCommand requestCommand)
        {
            return new MbResult<AccountDto?> { Status = HttpStatusCode.OK, Value = await _accountService.Add(requestCommand) };
        }

        /// <summary>
        /// Изменение данных счёта
        /// </summary>
        /// <param name="id">id счёта</param>
        /// <param name="requestCommand"> Данные счёта</param>
        /// <returns>Данные изменённого счёта</returns>
        [HttpPut("{id}")]
        public async Task<MbResult<AccountDto?>> Put(Guid id, [FromBody] UpdateAccountRequestCommand requestCommand)
        {
            return new MbResult<AccountDto?> { Status = HttpStatusCode.OK, Value = await _accountService.Update(id, requestCommand) };
        }

        /// <summary>
        /// Удаление счёта
        /// </summary>
        /// <param name="id">id счёта</param>
        /// <returns>Сообщение о том, удалён ли счёт или нет</returns>
        [HttpDelete("{id}")]
        public async Task<MbResult<string>> Delete(Guid id)
        {
            bool isDeleted = await _accountService.DeleteById(id);

            if (isDeleted)
                return new MbResult<string> { Status = HttpStatusCode.OK, Value = $"Пользователь с id={id} удалён" };
            else
                return new MbResult<string> { Status = HttpStatusCode.BadRequest, MbError = [$"Не получилось удалить пользователя с id={id}"] };
        }
    }
}