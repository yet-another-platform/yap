using Types.Interfaces.Model;

namespace Users.Domain.DataTransferObjects;

public class RegisterDto : IUsername, IEmail, IPassword
{
    /// <inheritdoc />
    public string Username { get; set; } = string.Empty;

    /// <inheritdoc />
    public string? Email { get; set; } = string.Empty;

    /// <inheritdoc />
    public string Password { get; set; } = string.Empty;
}