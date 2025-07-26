using Account_Service.Features.Users;

namespace Account_Service.ObjectStorage
{
    /// <summary>
    /// 
    /// </summary>
    public class UsersStorage
    {
        private static readonly List<User> Users = [new(Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e"), "Иван")];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<User?> Find(Guid id)
        {
            User? existedUser = await Task.Run(() => Users.Find(a => a.Id.Equals(id)));

            if (existedUser != null)
            {
                existedUser = new User(existedUser.Id, existedUser);
            }

            return existedUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<List<User>> FindAll()
        {
            return await Task.Run(() => Users.Select(user => new User(user.Id, user)).ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static async Task<User> Add(User user)
        {
            user = new User(Guid.NewGuid(), user);

            await Task.Run(() => Users.Add(user));

            return user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static async Task<User?> Update(User user)
        {
            if (user.Id != Guid.Empty)
            {
                User? existedUser = await Task.Run(() => Users.Find(a => a.Id.Equals(user.Id)));

                if (existedUser != null)
                {
                    existedUser.Name = user.Name;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<bool> Delete(Guid id)
        {
            User? existedUser = await Task.Run(() => Users.Find(a => a.Id.Equals(id)));

            if (existedUser != null)
            {
                return await Task.Run(() => Users.Remove(existedUser));
            }

            return false;
        }
    }
}