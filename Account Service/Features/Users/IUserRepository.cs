using Account_Service.Infrastructure.Repositories;

namespace Account_Service.Features.Users
// ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public interface IUserRepository : IRepository<User>;
}