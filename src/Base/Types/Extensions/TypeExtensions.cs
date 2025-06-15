using Types.Interfaces.Model;

namespace Types.Extensions;

public static class TypeExtensions
{
    public static bool IsSoftDeletable(this Type type) =>
        Array.Exists(type.GetInterfaces(), x => x == typeof(IDeleted) || x == typeof(IDeletedWithReason));
}