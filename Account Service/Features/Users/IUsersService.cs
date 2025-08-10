namespace Account_Service.Features.Users
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUsersService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserDto?> FindById(Guid id);
    }
}