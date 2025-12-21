using System.Reflection;
using Types.Exceptions;

namespace Service.Extensions;

public static class TypeExtensions
{
    internal static string GetSchemaName(this Type type)
    {
        var field = type.GetConstField("SchemaName");
        if (field == null)
        {
            throw new TypeException($"Type {type.Name} has no schema name constant field");
        }

        return field.GetValue(null) as string ?? throw new TypeException("Failed to get schema name");
    }

    public static FieldInfo? GetConstField(this Type type, string fieldName) =>
        type.GetField(fieldName, BindingFlags.Public | BindingFlags.Static);
}