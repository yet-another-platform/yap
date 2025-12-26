using System.Reflection;

namespace Service.Validation;

public class Validator<T>
{
    private static readonly Type Type = typeof(T);

    public ValidationResult Validate(T obj)
    {
        var result = new ValidationResult();
        foreach (var iface in Type.GetInterfaces())
        {
            var validateMethod = iface.GetMethod("Validate", BindingFlags.Public | BindingFlags.Static);

            if (validateMethod == null)
            {
                continue;
            }

            bool isValid = (bool)validateMethod.Invoke(null, [obj])!;
            if (isValid)
            {
                continue;
            }

            var property = iface.GetProperties().Single();

            result.Errors.Add(new ValidationError
            {
                PropertyName = property.Name,
                Message = $"Invalid {property.Name} format"
            });
        }

        return result;
    }
}