namespace Account_Service.Features.Users
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    public class User(Guid id, string name)
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; } = id;

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; } = name;

        /// <inheritdoc />
        public User(Guid id, User user) : this(id, user.Name)
        {
        }
    }
}