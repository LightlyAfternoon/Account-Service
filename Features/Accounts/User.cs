using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Account_Service.Features.Accounts
{
    public class User
    {
        [Required]
        [Column("id")]
        public Guid Id { get; }
        [Required]
        [Column("name")]
        public string Name { get; set; }

        public User(Guid id, User user)
        {
            Id = id;
            Name = user.Name;
        }
    }
}