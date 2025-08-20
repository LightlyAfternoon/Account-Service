using System.Net;
using System.Text.Json.Serialization;

namespace Account_Service.Infrastructure
// ReSharper disable once ArrangeNamespaceBody
{
    /// <summary>
    /// Класс для возврата в качестве результата HTTP запроса
    /// </summary>
    /// <typeparam name="TValue">Тип объекта тела ответа</typeparam>
    public class MbResult<TValue>(HttpStatusCode status)
    {
        /// <summary>
        /// Статус ответа
        /// </summary>
        public HttpStatusCode Status { get; } = status;

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

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is not MbResult<TValue> mbResult)
                return false;
            bool value;
            if (Value is IEnumerable<object> enumerable1 && mbResult.Value is IEnumerable<object> enumerable2 &&
                Value is not string && mbResult.Value is not string)
                value = enumerable1.SequenceEqual(enumerable2);
            else
                value = Value != null ? Value.Equals(mbResult.Value) : mbResult.Value == null;

            return Status.Equals(mbResult.Status)
                   && value
                   && (mbResult.MbError != null && MbError != null ? MbError.SequenceEqual(mbResult.MbError) : MbError == null && mbResult.MbError == null);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hash = Status.GetHashCode();

            return hash;
        }
    }
}