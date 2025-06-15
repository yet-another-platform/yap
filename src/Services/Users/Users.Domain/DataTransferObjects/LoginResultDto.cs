namespace Users.Domain.DataTransferObjects;

public class LoginResultDto
{
    public required string Token { get; set; }
    public required FullUserDto User { get; set; }
}