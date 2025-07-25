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

        /// <summary>
        /// Получение списка всех счетов
        /// </summary>
        /// <returns>Все счета из БД</returns>
        [HttpGet]
        public async Task<IResult> GetAllAccounts()
        {
            return Results.Json(await _accountService.FindAll());
        }

        /// <summary>
        /// Получение информации о том, есть или нет у данного счетов
        /// </summary>
        /// <param name="ownerId">id клиента</param>
        /// <param name="hasAnyAccount">параметр-маркер</param>
        /// <returns>Сообщение с информации о том, есть или нет у данного клиента счетов</returns>
        [HttpGet]
        public async Task<IResult> GetClientHasAnyAccount(Guid ownerId, bool hasAnyAccount)
        {
            if (await _accountService.ClientWithIdHasAnyAccount(ownerId))
                return Results.Ok(new { Message = "У данного пользователя есть счета" });
            else
                return Results.Ok(new { Message = "У данного пользователя нет счётов" });
        }

        /// <summary>
        /// Добавление нового счёта
        /// </summary>
        /// <param name="accountDto">Данные нового счёта</param>
        /// <returns>Данные счёта с новым id</returns>
        [HttpPost]
        public async Task<IResult> Post([FromBody] AccountDto accountDto)
        {
            return Results.Json(await _accountService.Add(accountDto));
        }

        /// <summary>
        /// Изменение данных счёта
        /// </summary>
        /// <param name="id">id счёта</param>
        /// <param name="accountDto"> Данные счёта</param>
        /// <returns>Данные изменённого счёта</returns>
        [HttpPut("{id}")]
        public async Task<IResult> Put(Guid id, [FromBody] AccountDto accountDto)
        {
            return Results.Json(await _accountService.Update(id, accountDto));
        }

        /// <summary>
        /// Удаление счёта
        /// </summary>
        /// <param name="id">id счёта</param>
        /// <returns>Сообщение о том, удалён ли счёт или нет</returns>
        [HttpDelete("{id}")]
        public async Task<IResult> Delete(Guid id)
        {
            bool isDeleted = await _accountService.DeleteById(id);

            if (isDeleted)
                return Results.Ok(new { Message = $"Пользователь с id={id} удалён" });
            else
                return Results.BadRequest(new { Message = $"Не получилось удалить пользователя с id={id}" });
        }
    }
}