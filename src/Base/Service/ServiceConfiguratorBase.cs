using System.Data;
using System.Text;
using System.Text.Json.Serialization;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Service.Extensions;
using Service.Interfaces;
using Types.Handlers;
using Types.Validation;

namespace Service;

public abstract class ServiceConfiguratorBase<TDbContext> : IServiceConfigurator where TDbContext : DbContext
{
    protected abstract string MigrationsAssembly { get; }
    protected abstract void ConfigureServices(WebApplicationBuilder builder);
    public WebApplicationBuilder Configure(WebApplicationBuilder builder)
    {
        ConfigureGeneral(builder);
        ConfigureDatabase(builder);
        ConfigureOther(builder);
        ConfigureServices(builder);
        return builder;
    }

    private void ConfigureGeneral(WebApplicationBuilder builder)
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

    private void ConfigureDatabase(WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        string databaseConnectionString = configuration.GetConnectionString("PostgreSQL") ??
                                          throw new InvalidConfigurationException(
                                              "PostgreSQL connection string not found");

        builder.Services.AddDbContext<TDbContext>(options =>
        {
            options.UseNpgsql(databaseConnectionString, optionsBuilder =>
            {
                optionsBuilder.MigrationsAssembly(MigrationsAssembly);
                optionsBuilder.MigrationsHistoryTable("__MigrationsHistory", typeof(TDbContext).GetSchemaName());
            });
            options.UseNpgsql(x => x.MigrationsAssembly(MigrationsAssembly));
        });

        DefaultTypeMap.MatchNamesWithUnderscores = true;
        SqlMapper.AddTypeHandler(new GuidCheckedTypeHandler());

        builder.Services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(databaseConnectionString));
        builder.Services.AddScoped<Func<IDbConnection>>(_ => () => new NpgsqlConnection(databaseConnectionString));
    }

    private static void ConfigureOther(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(typeof(Validator<>));
    }
}