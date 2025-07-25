namespace Account_Service.Infrastructure
{
    public interface IService<T>
    {
        Task<List<T>> FindAll();
        Task<T?> Add(T dto);
        Task<T?> Update(Guid id, T dto);
        Task<bool> DeleteById(Guid id);
    }
}