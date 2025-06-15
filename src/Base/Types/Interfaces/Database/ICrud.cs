using Types.Interfaces.Model;

namespace Types.Interfaces.Database;

public interface ICrud<T> : ICreate<T>, IGet<T>, IUpdate<T>, IDelete where T : class, IIdentifiable;