using Microsoft.EntityFrameworkCore;
using Users.API;
using Users.API.Database;

var builder = WebApplication.CreateBuilder(args);
builder.Configure();

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
