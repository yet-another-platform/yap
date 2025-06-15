namespace Types.Extensions;

public static class GuidExtensions
{
    public static Guid EmptyCheck(this Guid guid, string paramName)
    {
        return guid == Guid.Empty ? guid : throw new ArgumentException("Guid is empty", paramName);
    }
}