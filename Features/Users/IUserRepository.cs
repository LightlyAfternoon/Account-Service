using Account_Service.Infrastructure.Repositories;

namespace Account_Service.Features.Users
{
    /// <inheritdoc />
    public interface IUserRepository : IRepository<User>;
}