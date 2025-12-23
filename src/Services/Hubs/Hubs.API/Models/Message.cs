using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hubs.API.Constants.Database;
using Types.Interfaces.Model;

namespace Hubs.API.Models;

[Table(MessagesTable.TableName)]
public class Message : IIdentifiable<long>, IChannelIdentifiable, IUserIdentifiable, ICreated, IUpdated
{
    /// <inheritdoc />
    [Key]
    [Column(IIdentifiable.ColumnName)]
    public long Id { get; set; }

    /// <inheritdoc />
    [Column(IChannelIdentifiable.ColumnName)]
    [ForeignKey(ChannelsTable.TableName)]
    [Required]
    public required Guid ChannelId { get; set; }

    /// <inheritdoc />
    [Column(IUserIdentifiable.ColumnName)]
    [Required]
    public required Guid UserId { get; set; }

    /// <summary>
    /// Content of the message
    /// </summary>
    [Column(MessagesTable.Content)]
    [MaxLength(MessagesTable.ContentMaxLength)]
    public required string Content { get; set; }

    /// <inheritdoc />
    [Column(ICreated.ColumnName)]
    [Required]
    public required DateTimeOffset Created { get; set; }

    /// <inheritdoc />
    [Column(IUpdated.ColumnName)]
    public DateTimeOffset? Updated { get; set; }
}