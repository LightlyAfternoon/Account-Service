namespace Account_Service.Infrastructure
{
    public interface IRepository<T>
    {
        Task<T?> FindById(Guid id);
        Task<List<T>> FindAll();
        Task<T?> Save(T entity);
        Task<bool> DeleteById(Guid id);
    }
}