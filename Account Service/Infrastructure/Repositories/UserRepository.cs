using Account_Service.Features.Users;
using Account_Service.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Account_Service.Infrastructure.Repositories
// ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<User?> FindById(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        /// <inheritdoc />
        public async Task<List<User>> FindAll()
        {
            return await _context.Users.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<User?> Save(User entity, CancellationToken cancellationToken)
        {
            if (entity.Id == Guid.Empty)
            {
                await _context.Users.AddAsync(entity, cancellationToken);
            }
            else
            {
                _context.Users.Update(entity);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteById(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return false;

            _context.Users.Remove(user);

            return true;

        }
    }
}