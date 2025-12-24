using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Client.Extensions;
using Client.ViewModels;
using Client.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Client;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
#if DEBUG
        var loggerFactory = LoggerFactory.Create(b => b
            .SetMinimumLevel(LogLevel.Information)
            .AddConsole());
        this.AttachDeveloperTools(o =>
        {
            o.AddMicrosoftLoggerObservable(loggerFactory);
        });
#endif
    }

    public override void OnFrameworkInitializationCompleted()
    {
        BindingPlugins.DataValidators.Clear();
        
        var collection = new ServiceCollection().AddYapServices();
        var serviceProvider = collection.BuildServiceProvider();
        
        DataTemplates.Add(serviceProvider.GetRequiredService<ViewLocator>());
        var mainVm = serviceProvider.GetRequiredService<MainWindowViewModel>();
        var mainView = serviceProvider.GetRequiredService<MainWindowView>();
        mainVm.Initialize();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = mainView;
        }

        base.OnFrameworkInitializationCompleted();
    }
}