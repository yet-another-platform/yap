using Types.Interfaces.Model;

namespace Hubs.Domain.DataTransferObjects;

public class NewHubDto : IName
{
    public required string Name { get; set; }
}