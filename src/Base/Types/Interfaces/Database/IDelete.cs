namespace Types.Interfaces.Database;

public interface IDelete<in TId>
{
    public Task<bool> ExistsAsync(TId id);
}

public interface IDelete : IDelete<Guid>;