namespace Account_Service.Infrastructure
{
    public interface IService<T>
    {
        T? FindById(Guid id);
        List<T> FindAll();
        T? Add(T dto);
        T? Update(Guid id, T dto);
        bool DeleteById(Guid id);
    }
}