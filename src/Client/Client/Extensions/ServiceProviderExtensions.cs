using System;
using Client.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Extensions;

public static class ServiceProviderExtensions
{
    public static IView GetViewForViewModel(this IServiceProvider serviceProvider, string viewModelName)
    {
        return serviceProvider.GetRequiredKeyedService<IView>(viewModelName.Replace("ViewModel", "View", StringComparison.Ordinal));
    }
}