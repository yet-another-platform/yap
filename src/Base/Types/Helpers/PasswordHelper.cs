using static BCrypt.Net.BCrypt;

namespace Types.Helpers;

public static class PasswordHelper
{
    public static string GetPasswordHash(string password)
    {
        return HashPassword(password);
    }

    public static bool ValidatePassword(string password, string passwordHash)
    {
        return Verify(password, passwordHash);
    }
}