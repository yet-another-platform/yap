using Types.Interfaces.Model;

namespace Hubs.Domain.DataTransferObjects;

public class NewChannelDto : IName, IDescription
{
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
}
