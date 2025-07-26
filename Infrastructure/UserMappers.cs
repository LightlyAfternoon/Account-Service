using Account_Service.Features.Users;

namespace Account_Service.Infrastructure
{
    /// <inheritdoc />
    public class UserMappers : IMappers<UserDto, User>
    {
        /// <inheritdoc />
        public static UserDto MapToDto(User user) => new(id: user.Id,
            name: user.Name);

        /// <inheritdoc />
        public static User MapToEntity(UserDto userDto) => new(id: userDto.Id,
            userDto.Name);
    }
}