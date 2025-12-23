using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Types.Interfaces.Model;

namespace Hubs.Domain.DataTransferObjects;

public class NewMessageDto
{
    public string Content { get; set; } = string.Empty;
}