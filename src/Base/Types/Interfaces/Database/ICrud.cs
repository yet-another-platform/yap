using Types.Interfaces.Model;

namespace Types.Interfaces.Database;

public interface ICrud<TId, T> : ICreate<TId, T>, IGet<TId, T>, IUpdate<TId, T>, IDelete<TId> where T : class, IIdentifiable<TId>;
public interface ICrud<T> : ICreate<T>, IGet<T>, IUpdate<T>, IDelete where T : class, IIdentifiable;