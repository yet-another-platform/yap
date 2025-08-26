using System.Data;
using System.Text;
using System.Text.Json.Serialization;
using Dapper;
using Hubs.API.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Service.Interfaces;
using Types.Handlers;
using Types.Validation;

namespace Hubs.API;

public class ServiceConfigurator : IServiceConfigurator
{
    public WebApplicationBuilder Configure(WebApplicationBuilder builder)
    {
        ConfigureGeneral(builder);
        ConfigureDatabase(builder);
        ConfigureManagers(builder);
        ConfigureLocalServices(builder);
        return builder;
    }

    private static void ConfigureGeneral(WebApplicationBuilder builder)
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

    private static void ConfigureDatabase(WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        string databaseConnectionString = configuration.GetConnectionString("PostgreSQL") ??
                                          throw new InvalidConfigurationException(
                                              "PostgreSQL connection string not found");

        builder.Services.AddDbContext<HubsDatabaseContext>(options =>
        {
            options.UseNpgsql(databaseConnectionString, optionsBuilder =>
            {
                optionsBuilder.MigrationsAssembly("Hubs.API");
                optionsBuilder.MigrationsHistoryTable("__MigrationsHistory", HubsDatabaseContext.SchemaName);
            });
            options.UseNpgsql(x => x.MigrationsAssembly("Hubs.API"));
        });

        DefaultTypeMap.MatchNamesWithUnderscores = true;
        SqlMapper.AddTypeHandler(new GuidCheckedTypeHandler());

        builder.Services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(databaseConnectionString));
        builder.Services.AddScoped<Func<IDbConnection>>(_ => () => new NpgsqlConnection(databaseConnectionString));
    }

    private static void ConfigureManagers(WebApplicationBuilder builder)
    {
    }

    private static void ConfigureLocalServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<Validator>();
    }
}