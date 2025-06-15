using Users.Domain.DataTransferObjects;

namespace Users.API.Models.Convertors;

public static class UserConvertor
{
    public static FullUserDto ToFullDto(this User user)
    {
        return new FullUserDto
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
            State = user.State,
            Created = user.Created,
            Updated = user.Updated,
        };
    }
    
    public static SimpleUserDto ToSimpleDto(this User user)
    {
        return new SimpleUserDto
        {
            Id = user.Id,
            Username = user.Username,
        };
    }
}