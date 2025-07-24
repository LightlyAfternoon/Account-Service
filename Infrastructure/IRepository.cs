namespace Account_Service.Infrastructure
{
    public interface IRepository<T>
    {
        T? FindById(Guid id);
        List<T> FindAll();
        T? Save(T entity);
        bool DeleteById(Guid id);
    }
}