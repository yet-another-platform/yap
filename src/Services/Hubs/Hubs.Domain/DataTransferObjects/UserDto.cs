using Types.Interfaces.Model;

namespace Hubs.Domain.DataTransferObjects;

public class SimpleUserDto : IIdentifiable, IUsername
{
    /// <inheritdoc />
    public Guid Id { get; set; }

    /// <inheritdoc />
    public string Username { get; set; } = string.Empty;
}