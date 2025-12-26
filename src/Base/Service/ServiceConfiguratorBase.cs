using System.Data;
using System.Text;
using System.Text.Json.Serialization;
using Dapper;
using Database.Handlers;
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
using Types.Types;
using Types.Validation;

namespace Service;

public abstract class ServiceConfiguratorBase : IServiceConfigurator 
{
    protected abstract void ConfigureServices(WebApplicationBuilder builder);
    public virtual WebApplicationBuilder Configure(WebApplicationBuilder builder)
    {
        ConfigureGeneral(builder);
        ConfigureOther(builder);
        ConfigureServices(builder);
        return builder;
    }

    protected void ConfigureGeneral(WebApplicationBuilder builder)
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
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = ctx =>
                    {
                        var path = ctx.HttpContext.Request.Path;
                        if (!path.StartsWithSegments("/rt") || !string.IsNullOrWhiteSpace(ctx.Token))
                        {
                            return Task.CompletedTask;
                        }
                        var accessToken = ctx.Request.Query["access_token"];
                        ctx.Token = accessToken;
                            
                        return Task.CompletedTask;
                    }
                };
            });

        builder.Services.AddMvc();
    }

    protected static void ConfigureOther(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(typeof(Validator<>));
        builder.Services.AddScoped<CorrelationId>();
    }
}

public abstract class ServiceConfiguratorWithDatabaseBase<TDbContext> : ServiceConfiguratorBase where TDbContext : DbContext
{
    protected abstract string MigrationsAssembly { get; }
    public override WebApplicationBuilder Configure(WebApplicationBuilder builder)
    {
        ConfigureGeneral(builder);
        ConfigureOther(builder);
        ConfigureServices(builder);
        ConfigureDatabase(builder);
        return builder;
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
}