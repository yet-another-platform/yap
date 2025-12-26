using System.Windows.Input;
using Client.Interfaces;

namespace Client.ViewModels.Pages;

public class FirstPageViewModel(INavigationService navigationService) : ViewModelBase<FirstPageViewModel>
{
    public ICommand GoToOtherCommand { get; } = new Command(navigationService.GoToPage<OtherPageViewModel>);
}