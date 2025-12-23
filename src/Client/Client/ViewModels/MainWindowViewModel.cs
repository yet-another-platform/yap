using System;
using Client.Interfaces;
using Client.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Client.ViewModels;

public class MainWindowViewModel(IServiceProvider serviceProvider): ViewModelBase<MainWindowViewModel>, INavigationService
{
    private IViewModel _currentPage = null!;
    public IViewModel CurrentPage
    {
        get => _currentPage;
        set => this.RaiseAndSetIfChanged(ref _currentPage, value);
    }

    public void Initialize()
    {
        CurrentPage = serviceProvider.GetRequiredService<LoginPageViewModel>();
    }
    public void GoToPage<TViewModel>() where TViewModel : ViewModelBase<TViewModel>
    {
        CurrentPage = serviceProvider.GetRequiredService<TViewModel>();
    }
}