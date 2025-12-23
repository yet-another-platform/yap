using System.Diagnostics.CodeAnalysis;
using Client.Interfaces;
using Client.ViewModels;
using Client.ViewModels.Controls;
using Client.ViewModels.Pages;
using Client.Views.Controls;
using Client.Views.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        return services
            .AddPages()
            .AddControls()
            .AddSingleton<INavigationService>(sp => sp.GetRequiredService<MainWindowViewModel>())
            .AddSingleton<ViewLocator>();
    }

    private static IServiceCollection AddPages(this IServiceCollection services) =>
        services
            .AddSingleton<MainWindowViewModel>()
            .RegisterViewViewModelPair<LoginPageView, LoginPageViewModel>();

    private static IServiceCollection AddControls(this IServiceCollection services) =>
        services
            .RegisterViewViewModelPair<LoginFormView, LoginFormViewModel>()
            .RegisterViewViewModelPair<RegisterFormView, RegisterFormViewModel>();

    private static IServiceCollection RegisterViewViewModelPair<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TView,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>(this IServiceCollection services)
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