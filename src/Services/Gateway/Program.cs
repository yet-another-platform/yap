using Gateway;
using Ocelot.Middleware;

var app = await BuildWebHost(args);
await app.RunAsync();
return;

static async Task<WebApplication> BuildWebHost(string[] args)
{
    var builder = WebApplication.CreateBuilder(args).ConfigureBuilder();
    var app = builder.Build().ConfigureApp();
    await app.UseOcelot();
    return app;
}