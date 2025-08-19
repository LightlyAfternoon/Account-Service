using System.Text.Json.Serialization;

namespace Account_Service.Features.Users
// ReSharper disable once ArrangeNamespaceBody
{
    /// <summary>
    /// DTO пользователя
    /// </summary>
    /// <param name="id">Id пользователя</param>
    /// <param name="name">Имя пользователя</param>
    [method: JsonConstructor]
    public class UserDto(Guid id, string name)
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public Guid Id { get; } = id;
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; } = name;

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is not UserDto user)
                return false;
            return Id.Equals(user.Id) && Name.Equals(user.Name);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}