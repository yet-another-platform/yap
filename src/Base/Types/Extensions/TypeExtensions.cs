using System.Diagnostics.CodeAnalysis;
using Types.Interfaces.Model;

namespace Types.Extensions;

public static class TypeExtensions
{
    public static bool IsSoftDeletable([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] this Type type) =>
        Array.Exists(type.GetInterfaces(), x => x == typeof(IDeleted) || x == typeof(IDeletedWithReason));
}