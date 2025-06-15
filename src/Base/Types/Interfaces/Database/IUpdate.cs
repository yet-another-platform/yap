using Types.Interfaces.Model;

namespace Types.Interfaces.Database;

public interface IUpdate<T> where T : class, IIdentifiable
{
    public Task<bool> UpdateAsync(T entity);
}