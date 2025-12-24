using System.Threading.Tasks;
using Users.Domain.DataTransferObjects;

namespace Client.Models.Interfaces;

public interface IAuthSession
{
    public FullUserDto? CurrentUser { get; }
    public string Token { get; }
    public bool IsLoggedIn { get; }

    public Task LoginAsync(string identifier, string password);
    public Task LogoutAsync();
}