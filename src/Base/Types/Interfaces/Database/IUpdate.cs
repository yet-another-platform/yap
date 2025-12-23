using Types.Interfaces.Model;

namespace Types.Interfaces.Database;

public interface IUpdate<TId, in T> where T : class, IIdentifiable<TId>
{
    public Task<bool> UpdateAsync(T entity);
}

public interface IUpdate<in T> : IUpdate<Guid, T> where T : class, IIdentifiable<Guid>;
