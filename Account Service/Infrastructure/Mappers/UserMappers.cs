using Account_Service.Features.Users;

namespace Account_Service.Infrastructure.Mappers
// ReSharper disable once ArrangeNamespaceBody
{
    /// <summary>
    /// 
    /// </summary>
    public class UserMappers
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static UserDto MapToDto(User user) => new(id: user.Id,
            name: user.Name);
    }
}