using Types.Interfaces.Model;
using Types.Types.Option;

namespace Types.Interfaces.Database;

public interface IGet<T> where T : IIdentifiable
{
    public Task<Option<T>> GetAsync(Guid id);
}