using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Types.Helpers;
using Types.Types;
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
    Validator<RegisterDto> registerValidator,
    Validator<User> userValidator)
{
    public async Task<Option<LoginResultDto>> RegisterAsync(RegisterDto registerDto)
    {
        var validationResult = registerValidator.Validate(registerDto);
        if (!validationResult.IsValid)
        {
            return new Error
                { Message = string.Join('\n', validationResult.Errors.Select(e => e.Message)) };
        }
        
        var existingUser = await userDatabaseService.GetByEmailAsync(registerDto.Email!, true);
        if (existingUser is not null)
        {
            return new Error
            {
                Message = "Email is already registered",
                Type = ErrorType.AlreadyExists
            };
        }
        
        existingUser = await userDatabaseService.GetByUsernameAsync(registerDto.Username, true);
        if (existingUser is not null)
        {
            return new Error
            {
                Message = "Username is already registered",
                Type = ErrorType.AlreadyExists
            };
        }

        var user = new User
        {
            Email = registerDto.Email,
            Username = registerDto.Username,
            PasswordHash = PasswordHelper.GetPasswordHash(registerDto.Password),
            State = UserState.Activated
        };

        user.Updated = user.Created = DateTimeOffset.UtcNow;

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

        return new LoginResultDto
        {
            Token = tokenResult.Value,
            User = user.ToFullDto()
        };
    }

    public async Task<Option<LoginResultDto>> LoginAsync(LoginDto loginDto)
    {
        if (string.IsNullOrWhiteSpace(loginDto.Identifier))
        {
            return new Error
            {
                Message = "Invalid login identifier",
                Type = ErrorType.BadRequest
            };
        }

        if (string.IsNullOrWhiteSpace(loginDto.Password))
        {
            return new Error
            {
                Message = "Invalid login password",
                Type = ErrorType.BadRequest
            };
        }

        var user = await userDatabaseService.GetByEmailAsync(loginDto.Identifier);
        if (user == null)
        {
            user = await userDatabaseService.GetByUsernameAsync(loginDto.Identifier);
        }

        if (user is null)
        {
            return new Error
            {
                Message = "User not found",
                Type = ErrorType.NotFound
            };
        }
        
        bool isCorrectPassword = PasswordHelper.ValidatePassword(loginDto.Password, user.PasswordHash!);
        if (!isCorrectPassword)
        {
            return new Error
            {
                Message = "Wrong password",
                Type = ErrorType.BadRequest
            };
        }
        
        var tokenResult = GenerateJwt(user);
        if (!tokenResult.Ok)
        {
            return new Error
            {
                Message = "Failed to create token",
                Type = ErrorType.ServiceError
            };
        }

        return new LoginResultDto
        {
            User = user.ToFullDto(),
            Token = tokenResult.Value
        };
    }

    public async Task<Option<User>> GetAsync(GuidChecked userId)
    {
       var user = await userDatabaseService.GetAsync(userId);
       if (user is null)
       {
           return new Error
           {
               Message = "User not found",
               Type = ErrorType.NotFound
           };
       }

       return user;
    }

    private Option<string> GenerateJwt(User user)
    {
        var validationResult = userValidator.Validate(user);
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