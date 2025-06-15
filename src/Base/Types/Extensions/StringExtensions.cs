namespace Types.Extensions;

public static class StringExtensions
{
    public static bool IsAsciiOnly(this string value) => value.All(char.IsAscii);
}