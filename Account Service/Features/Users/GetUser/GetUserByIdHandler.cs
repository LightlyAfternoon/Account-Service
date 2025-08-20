using Account_Service.Infrastructure.Mappers;
using MediatR;

namespace Account_Service.Features.Users.GetUser
// ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequestCommand, UserDto?>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRepository"></param>
        public GetUserByIdHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public async Task<UserDto?> Handle(GetUserByIdRequestCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindById(request.Id);

            return user != null ? UserMappers.MapToDto(user) : null;
        }
    }
}