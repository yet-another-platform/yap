using Types.Types;

namespace Types.Interfaces.Database;

public interface IExists<in TId>
{
    public Task<bool> Exists(TId id);
}

public interface IExists : IExists<GuidChecked>;