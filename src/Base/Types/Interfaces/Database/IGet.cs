using Types.Interfaces.Model;
using Types.Types;
using Types.Types.Option;

namespace Types.Interfaces.Database;

public interface IGet<in TId, T>
{
    public Task<Option<T>> GetAsync(TId id);
}

public interface IGet<T> : IGet<GuidChecked, T> where T : IIdentifiable;