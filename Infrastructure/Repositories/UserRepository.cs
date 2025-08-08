using Account_Service.Features.Users;
using Account_Service.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Account_Service.Infrastructure.Repositories
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
            await using ApplicationContext db = new ApplicationContext(_context.ConnectionString);
            return await db.Users.FindAsync(id);
        }

        /// <inheritdoc />
        public async Task<List<User>> FindAll()
        {
            await using ApplicationContext db = new ApplicationContext(_context.ConnectionString);
            return await db.Users.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<User?> Save(User entity)
        {
            if (entity.Id == Guid.Empty)
            {
                await using ApplicationContext db = new ApplicationContext(_context.ConnectionString);
                await db.Users.AddAsync(entity);
                await db.SaveChangesAsync();

                return entity;
            }
            else
            {
                await using ApplicationContext db = new ApplicationContext(_context.ConnectionString);
                db.Users.Update(entity);
                await db.SaveChangesAsync();

                return entity;
            }
        }

        /// <inheritdoc />
        public async Task<bool> DeleteById(Guid id)
        {
            await using ApplicationContext db = new ApplicationContext(_context.ConnectionString);
            User? user = await db.Users.FindAsync(id);

            if (user != null)
            {
                db.Users.Remove(user);

                return true;
            }

            return false;
        }
    }
}