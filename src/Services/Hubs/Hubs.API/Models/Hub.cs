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
    [Required]
    public required Guid UserId { get; set; }

    /// <inheritdoc />
    [Column(IName.ColumnName)]
    [MaxLength(IName.MaxLength)]
    [Required]
    public required string Name { get; set; }

    /// <inheritdoc />
    [Column(ICreated.ColumnName)]
    [Required]
    public DateTimeOffset Created { get; set; }

    /// <inheritdoc />
    [Column(IUpdated.ColumnName)]
    public DateTimeOffset? Updated { get; set; }

    [NotMapped]
    public List<Channel> ChannelList { get; set; } = [];

    [NotMapped]
    public List<HubMembership> MemberList { get; set; } = [];
}