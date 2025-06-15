using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Types.Enums;
using Types.Interfaces.Model;
using Users.API.Constants.Database;
using Users.Domain.Enums;

namespace Users.API.Models;

[Table(UsersTable.TableName)]
public class User : IIdentifiable, IEmail, IUsername, ICreated, IUpdated
{
    /// <inheritdoc />
    [Key]
    [Column(IIdentifiable.ColumnName)]
    public Guid Id { get; set; } = Guid.Empty;
    
    /// <inheritdoc />
    [Required]
    [MaxLength(IEmail.MaxLength)]
    [Column(IEmail.ColumnName)]
    public string? Email { get; set; }
    
    /// <inheritdoc />
    [Required]
    [MaxLength(IUsername.MaxLength)]
    [Column(IUsername.ColumnName)]
    public required string Username { get; set; }
    
    /// <summary>
    /// Hash of the password set by the user
    /// </summary>
    [MaxLength(1024)]
    [Column(UsersTable.PasswordHash)]
    public string? PasswordHash { get; set; }
   
    /// <summary>
    /// State of the user
    /// </summary>
    [Column(UsersTable.State)]
    public UserState State { get; set; }
    
    /// <inheritdoc />
    [Required]
    [Column(ICreated.ColumnName)]
    public DateTimeOffset Created { get; set; }
    
    /// <inheritdoc />
    [Column(IUpdated.ColumnName)]
    public DateTimeOffset? Updated { get; set; }
}