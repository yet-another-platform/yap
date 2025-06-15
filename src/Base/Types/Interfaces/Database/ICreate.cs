using Types.Interfaces.Model;

namespace Types.Interfaces.Database;

public interface ICreate<in T> where T : class, IIdentifiable
{
    public Task<Guid> CreateAsync(T entity);
}