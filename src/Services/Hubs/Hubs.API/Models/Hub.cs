using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hubs.API.Constants.Database;
using Types.Interfaces.Model;

namespace Hubs.API.Models;

[Table(HubsTable.TableName)]
public class Hub : IIdentifiable, IUserIdentifiable, IName, ICreated, IUpdated
{
    /// <inheritdoc />
    [Key]
    [Column(IIdentifiable.ColumnName)]
    public Guid Id { get; set; }
    
    /// <inheritdoc />
    [Column(IUserIdentifiable.ColumnName)]
    public Guid UserId { get; set; }
    
    /// <inheritdoc />
    [Column(IName.ColumnName)]
    public string Name { get; set; } = string.Empty;
    
    /// <inheritdoc />
    [Column(ICreated.ColumnName)]
    public DateTimeOffset Created { get; set; }
    
    /// <inheritdoc />
    [Column(IUpdated.ColumnName)]
    public DateTimeOffset? Updated { get; set; }
}