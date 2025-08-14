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

        /// <summary>
        /// 
        /// </summary>
        public uint RowVersion { get; set; }

        /// <inheritdoc />
        public User(Guid id, User user) : this(id, user.Name)
        {
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            var user = obj as User;

            if (user == null)
                return false;
            else
                return Id.Equals(user.Id) && Name.Equals(user.Name);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}