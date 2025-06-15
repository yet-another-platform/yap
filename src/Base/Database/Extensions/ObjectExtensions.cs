using Dapper;

namespace Database.Extensions;

public static class ObjectExtensions
{
    public static DynamicParameters ToDynamicParameters(this object obj) => new DynamicParameters(obj);
}