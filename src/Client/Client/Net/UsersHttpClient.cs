using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Types.Types.Option;
using Users.Domain.DataTransferObjects;

namespace Client.Net;

public class UsersHttpClient(ILogger<UsersHttpClient> logger, HttpClient httpClient, JsonContext jsonContext) : HttpClientBase(logger, httpClient)
{
    public async Task<Option<LoginResultDto>> LoginAsync(string username, string password)
    {
        var loginDto = new LoginDto
        {
            Identifier = username,
            Password = password
        };

        return await SendRequestWithResponseAsync<LoginResultDto>(
            client => client.PostAsJsonAsync("/api/users/user/login", loginDto, jsonTypeInfo: jsonContext.LoginDto),
            jsonContext.LoginResultDto);
    }

    public async Task<Option<LoginResultDto>> RegisterAsync(string email, string username, string password)
    {
        var registerDto = new RegisterDto
        {
            Email = email,
            Username = username,
            Password = password
        };

        return await SendRequestWithResponseAsync<LoginResultDto>(
            client => client.PostAsJsonAsync("/api/users/user/register", registerDto, jsonTypeInfo: jsonContext.RegisterDto),
            jsonTypeInfo: jsonContext.LoginResultDto);
    }
}