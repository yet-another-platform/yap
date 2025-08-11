using Microsoft.EntityFrameworkCore;
using Service.Extensions;
using Users.API;
using Users.API.Database;

var builder = WebApplication.CreateBuilder(args);
await builder.ConfigureAsync<ServiceConfigurator>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<UsersDatabaseContext>();
    
    await dbContext.Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
await app.RunAsync();
