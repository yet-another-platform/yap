using System.Windows.Input;
using Client.Interfaces;

namespace Client.ViewModels.Pages;

public class OtherPageViewModel(INavigationService navigationService) : ViewModelBase<OtherPageViewModel>
{
    public ICommand GoToFirstCommand { get; } = new Command(navigationService.GoToPage<FirstPageViewModel>);
}