using Account_Service.Features.Users;
using Account_Service.ObjectStorage;

namespace Account_Service.Infrastructure
{
    /// <inheritdoc />
    public class UserRepository : IUserRepository
    {
        /// <inheritdoc />
        public async Task<User?> FindById(Guid id)
        {
            return await UsersStorage.Find(id);
        }

        /// <inheritdoc />
        public async Task<List<User>> FindAll()
        {
            return await UsersStorage.FindAll();
        }

        /// <inheritdoc />
        public async Task<User?> Save(User entity)
        {
            if (entity.Id == Guid.Empty)
            {
                return await UsersStorage.Add(entity);
            }
            else
            {
                return await UsersStorage.Update(entity);
            }
        }

        /// <inheritdoc />
        public async Task<bool> DeleteById(Guid id)
        {
            return await UsersStorage.Delete(id);
        }
    }
}