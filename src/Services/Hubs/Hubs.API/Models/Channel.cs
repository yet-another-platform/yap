using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hubs.API.Constants.Database;
using Types.Interfaces.Model;

namespace Hubs.API.Models;

[Table(ChannelsTable.TableName)]
public class Channel : IIdentifiable, IHubIdentifiable, IName, IDescription, ICreated, IUpdated
{
    /// <inheritdoc />
    [Key]
    [Column(IIdentifiable.ColumnName)]
    public Guid Id { get; set; }
    
    /// <inheritdoc />
    [Column(IHubIdentifiable.ColumnName)]
    [ForeignKey(HubsTable.TableName)]
    [Required]
    public required Guid HubId { get; set; }
    
    /// <inheritdoc />
    [Column(IName.ColumnName)]
    [MaxLength(IName.MaxLength)]
    [Required]
    public required string Name { get; set; }
    
    /// <inheritdoc />
    [Column(IDescription.ColumnName)]
    [MaxLength(IDescription.MaxLength)]
    public string Description { get; set; } = string.Empty;
    
    /// <inheritdoc />
    [Required]
    [Column(ICreated.ColumnName)]
    public required DateTimeOffset Created { get; set; }
    
    /// <inheritdoc />
    [Column(IUpdated.ColumnName)]
    public DateTimeOffset? Updated { get; set; }
    
    [NotMapped]
    public List<Message> MessageList { get; set; } = [];
}