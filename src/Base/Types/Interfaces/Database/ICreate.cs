using Types.Interfaces.Model;

namespace Types.Interfaces.Database;

public interface ICreate<TId, in T> where T : class, IIdentifiable<TId>
{
    public Task<TId> CreateAsync(T entity);
}

public interface ICreate<in T> : ICreate<Guid, T> where T : class, IIdentifiable<Guid>;