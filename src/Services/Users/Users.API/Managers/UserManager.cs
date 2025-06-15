using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Types.Helpers;
using Types.Types.Option;
using Types.Validation;
using Users.API.DatabaseServices.Interfaces;
using Users.API.Models;
using Users.API.Models.Convertors;
using Users.Domain.DataTransferObjects;
using Users.Domain.Enums;

namespace Users.API.Managers;

public class UserManager(
    ILogger<UserManager> logger,
    IConfiguration configuration,
    IUserDatabaseService userDatabaseService,
    Validator validator)
{
    public async Task<Option<RegistrationResultDto>> RegisterAsync(RegisterDto registerDto)
    {
        var validationResult = validator.Validate(registerDto);
        if (!validationResult.IsValid)
        {
            return new Error
                { Message = string.Join('\n', validationResult.Errors.Select(e => e.Message)) };
        }

        var user = new User
        {
            Email = registerDto.Email,
            Username = registerDto.Username,
            PasswordHash = PasswordHelper.GetPasswordHash(registerDto.Password),
            State = UserState.NotActivated,
            Created = DateTimeOffset.UtcNow
        };

        var createResult = await userDatabaseService.CreateAsync(user);
        if (createResult == Guid.Empty)
        {
            logger.LogError("Failed to create new user with username: {Username}", user.Username);
            return new Error { Message = "Failed to create a new user" };
        }

        user.Id = createResult;
        var tokenResult = GenerateJwt(user);
        if (!tokenResult.Ok)
        {
            logger.LogError("Failed to create a token for user: {UserID}", user.Id);
            return new Error { Message = "Failed to create a token" };
        }

        return new RegistrationResultDto
        {
            Token = tokenResult.Value,
            User = user.ToFullDto()
        };
    }

    private Option<string> GenerateJwt(User user)
    {
        var validationResult = validator.Validate(user);
        if (!validationResult.IsValid)
        {
            return new Option<string>(new Error
                { Message = string.Join('\n', validationResult.Errors.Select(e => e.Message)) });
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);

        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            //TODO: FIX THIS!!
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Issuer"],
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}