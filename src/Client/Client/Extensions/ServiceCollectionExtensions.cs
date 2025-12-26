using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Client.Interfaces;
using Client.Models;
using Client.Models.Interfaces;
using Client.Net;
using Client.ViewModels;
using Client.ViewModels.Controls;
using Client.ViewModels.Pages;
using Client.Views;
using Client.Views.Controls;
using Client.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddYapServices(this IServiceCollection services)
    {
        return services
            .AddPages()
            .AddControls()
            .AddModels()
#if DEBUG
            .AddLogging(config =>
            {
                config.ClearProviders();
                config.AddConsole();
                config.SetMinimumLevel(LogLevel.Debug);
            })
#endif
            .AddHttpClients()
            .AddSingleton<INavigationService>(sp => sp.GetRequiredService<MainWindowViewModel>())
            .AddSingleton<JsonContext>(new JsonContext(new JsonSerializerOptions
                { PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
            .AddSingleton<RealtimeManager>()
            .AddSingleton<ViewLocator>();
    }

    private static IServiceCollection AddPages(this IServiceCollection services) =>
        services
            .AddSingleton<MainWindowViewModel>()
            .AddSingleton<MainWindowView>(sp => new MainWindowView() { DataContext = sp.GetRequiredService<MainWindowViewModel>() })
            .RegisterViewViewModelPair<LoginPageView, LoginPageViewModel>();

    private static IServiceCollection AddControls(this IServiceCollection services) =>
        services
            .RegisterViewViewModelPair<LoginFormView, LoginFormViewModel>()
            .RegisterViewViewModelPair<RegisterFormView, RegisterFormViewModel>();

    private static IServiceCollection AddModels(this IServiceCollection services) =>
        services
            .AddSingleton<IAuthSession, AuthSession>();

    private static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient<UsersHttpClient>((_, client) =>
        {
            client.BaseAddress = new Uri("http://localhost:40000");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "Yap-Client");
        });
        return services;
    }

    private static IServiceCollection RegisterViewViewModelPair<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TView,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TViewModel>(this IServiceCollection services)
        where TView : class, IView where TViewModel : class, IViewModel
    {
        services
            .AddTransient<TViewModel>()
            .AddTransient<IViewModel, TViewModel>()
            .AddKeyedTransient<TViewModel>(typeof(TViewModel).Name)
            .AddKeyedTransient<IViewModel, TViewModel>(typeof(TViewModel).Name);

        services
            .AddTransient<TView>()
            .AddTransient<IView, TView>()
            .AddKeyedTransient<TView>(typeof(TView).Name)
            .AddKeyedTransient<IView, TView>(typeof(TView).Name);
        return services;
    }
}