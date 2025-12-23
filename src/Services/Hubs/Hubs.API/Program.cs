using Hubs.API;
using Hubs.API.Database;
using Microsoft.EntityFrameworkCore;
using Service.Extensions;

var builder = WebApplication.CreateBuilder(args);
await builder.ConfigureAsync<ServiceConfigurator>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<HubsDatabaseContext>();
    
    await dbContext.Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
await app.RunAsync();
