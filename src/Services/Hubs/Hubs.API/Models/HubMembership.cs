using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hubs.API.Constants.Database;
using Types.Interfaces.Model;

namespace Hubs.API.Models;

[Table(HubMembershipsTable.TableName)]
public class HubMembership : IIdentifiable, IHubIdentifiable, IUserIdentifiable, ICreated
{
    /// <inheritdoc />
    [Key]
    [Column(IIdentifiable.ColumnName)]
    public Guid Id { get; set; }

    /// <inheritdoc />
    [Column(IHubIdentifiable.ColumnName)]
    public Guid HubId { get; set; }

    /// <inheritdoc />
    [Column(IUserIdentifiable.ColumnName)]
    public Guid UserId { get; set; }

    /// <inheritdoc />
    [Required]
    [Column(ICreated.ColumnName)]
    public DateTimeOffset Created { get; set; }
}