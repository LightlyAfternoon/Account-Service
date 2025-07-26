using System.Text.Json.Serialization;

namespace Account_Service.Features.Users
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    [method: JsonConstructor]
    public class UserDto(Guid id, string name)
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; } = id;
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; } = name;
    }
}