using Hubs.Domain.Enums;
using Types.Interfaces.Model;

namespace Hubs.Domain.DataTransferObjects;

public class FullUserDto : SimpleUserDto, IEmail, ICreated, IUpdated
{
    /// <inheritdoc />
    public string? Email { get; set; } = string.Empty;
    
    /// <inheritdoc />
    public DateTimeOffset Created { get; set; }
    
    /// <inheritdoc />
    public DateTimeOffset? Updated { get; set; }
    
    /// <summary>
    /// State of the user
    /// </summary>
    public UserState State { get; set; }
}