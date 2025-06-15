namespace Users.Domain.DataTransferObjects;

public class RegistrationResultDto
{
    public required string Token { get; set; }
    public required FullUserDto User { get; set; }
}