using RealTime;
using RealTime.Hubs;
using Service.Extensions;

var builder = WebApplication.CreateBuilder(args);
await builder.ConfigureAsync<ServiceConfigurator>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapHub<ChatHub>("/rt/chat");

await app.RunAsync();