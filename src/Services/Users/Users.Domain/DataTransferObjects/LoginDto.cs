using Types.Interfaces.Model;

namespace Users.Domain.DataTransferObjects;

public class LoginDto : IPassword
{
    /// <summary>
    /// Identifier of user, either email or username
    /// </summary>
    public string Identifier { get; set; } = string.Empty;

    /// <inheritdoc />
    public string Password { get; set; } = string.Empty;
}