using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Required]
        [Column("id")]
        public Guid Id { get; } = id;

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Column("name")]
        public string Name { get; set; } = name;

        /// <inheritdoc />
        public User(Guid id, User user) : this(id, user.Name)
        {
        }
    }
}