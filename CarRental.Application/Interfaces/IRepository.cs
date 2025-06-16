namespace CarRental.Application.Interfaces;

public interface IRepository<T>
{
    IEnumerable<T> GetAll();
    T? Get(int id);
    void Add(T entity);
    void Delete(int id);
}