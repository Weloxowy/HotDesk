namespace HotDeskApp.Server.Infrastructure;

public interface IRepository<T> where T : Entity
{
    Task<T> Get(Guid id);
    Task<IEnumerable<T>> GetAll();
    Task<Guid> Save(T entity);
    Task Update(T entity);
    Task Delete(Guid id);
}