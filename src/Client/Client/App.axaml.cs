using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Client.Extensions;
using Client.ViewModels;
using Client.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Client;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        BindingPlugins.DataValidators.Clear();
        
        var collection = new ServiceCollection();
        collection.AddAppServices();
        var serviceProvider = collection.BuildServiceProvider();
        
        DataTemplates.Add(serviceProvider.GetRequiredService<ViewLocator>());
        var mainVm = serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainVm.Initialize();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainVm,
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}