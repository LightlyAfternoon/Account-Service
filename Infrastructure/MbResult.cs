using System.Net;
using System.Text.Json.Serialization;

namespace Account_Service.Infrastructure
{
    /// <summary>
    /// Класс для возврата в качестве результата HTTP запроса
    /// </summary>
    /// <typeparam name="TValue">Тип объекта тела ответа</typeparam>
    public class MbResult<TValue>
    {
        /// <summary>
        /// Статус ответа
        /// </summary>
        public HttpStatusCode Status { get; set; }

        /// <summary>
        /// Тело ответа
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TValue? Value { get; set; }

        /// <summary>
        /// Ошибки при выполнении запроса
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? MbError { get; set; }
    }
}