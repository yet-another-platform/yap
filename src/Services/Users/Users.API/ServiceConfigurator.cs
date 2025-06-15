using System.Data;
using System.Text;
using System.Text.Json.Serialization;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Types.Handlers;
using Types.Validation;
using Users.API.Database;
using Users.API.DatabaseServices;
using Users.API.DatabaseServices.Interfaces;
using Users.API.Managers;

namespace Users.API;

public static class ServiceConfigurator
{
    public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
    {
        builder.ConfigureGeneral();
        builder.ConfigureDatabase();
        builder.ConfigureManagers();
        builder.ConfigureLocalServices();
        return builder;
    }

    private static void ConfigureGeneral(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.AddControllers()
            .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Issuer"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new InvalidOperationException()))
                };
            });

        builder.Services.AddMvc();
    }

    private static void ConfigureDatabase(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        string databaseConnectionString = configuration.GetConnectionString("PostgreSQL") ??
                                          throw new InvalidConfigurationException(
                                              "PostgreSQL connection string not found");

        builder.Services.AddDbContext<UsersDatabaseContext>(options =>
        {
            options.UseNpgsql(databaseConnectionString, optionsBuilder =>
            {
                optionsBuilder.MigrationsAssembly("Users.API");
                optionsBuilder.MigrationsHistoryTable("__MigrationsHistory", UsersDatabaseContext.SchemaName);
            });
            options.UseNpgsql(x => x.MigrationsAssembly("Users.API"));
        });

        DefaultTypeMap.MatchNamesWithUnderscores = true;
        SqlMapper.AddTypeHandler(new GuidCheckedTypeHandler());

        builder.Services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(databaseConnectionString));
        builder.Services.AddScoped<Func<IDbConnection>>(_ => () => new NpgsqlConnection(databaseConnectionString));

        // Database services
        builder.Services.AddScoped<IUserDatabaseService, UserDatabaseService>();
    }

    private static void ConfigureManagers(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<UserManager>();
    }

    private static void ConfigureLocalServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<Validator>();
    }
}