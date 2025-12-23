using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hubs.API.Constants.Database;
using Types.Interfaces.Model;

namespace Hubs.API.Models;

[Table(ChannelMembershipsTable.TableName)]
public class ChannelMembership : IIdentifiable, IChannelIdentifiable, IUserIdentifiable, ICreated
{
    /// <inheritdoc />
    [Key]
    [Column(IIdentifiable.ColumnName)]
    public Guid Id { get; set; }

    /// <inheritdoc />
    [Column(IChannelIdentifiable.ColumnName)]
    [ForeignKey(ChannelsTable.TableName)]
    public Guid ChannelId { get; set; }

    /// <inheritdoc />
    [Column(IUserIdentifiable.ColumnName)]
    public Guid UserId { get; set; }

    /// <inheritdoc />
    [Required]
    [Column(ICreated.ColumnName)]
    public DateTimeOffset Created { get; set; }
}