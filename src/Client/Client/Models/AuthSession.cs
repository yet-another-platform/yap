using System.Threading.Tasks;
using Client.Models.Interfaces;
using Client.Net;
using Users.Domain.DataTransferObjects;

namespace Client.Models;

public class AuthSession(UsersHttpClient usersHttpClient) : IAuthSession
{
    public FullUserDto? CurrentUser { get; private set; }
    public string Token { get; private set; } = string.Empty;
    
    public bool IsLoggedIn => CurrentUser != null && !string.IsNullOrWhiteSpace(Token);
    public async Task LoginAsync(string identifier, string password)
    {
        var result = await usersHttpClient.LoginAsync(identifier, password);
        if (!result.Ok)
        {
            return;
        }

        CurrentUser = result.Value.User;
        Token = result.Value.Token;
    }

    public Task LogoutAsync()
    {
        throw new System.NotImplementedException();
    }
}