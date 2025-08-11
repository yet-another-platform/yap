using Microsoft.AspNetCore.Builder;

namespace Service.Interfaces;

public interface IServiceConfigurator
{
    public WebApplicationBuilder Configure(WebApplicationBuilder builder);
}