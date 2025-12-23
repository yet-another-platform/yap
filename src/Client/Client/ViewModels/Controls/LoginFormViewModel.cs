using System.Windows.Input;
using Client.ViewModels.Pages;

namespace Client.ViewModels.Controls;

public class LoginFormViewModel(LoginPageViewModel loginPageViewModel) : ViewModelBase<LoginFormViewModel>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    
    public ICommand SwitchFormsCommand { get; } = new Command(loginPageViewModel.SwitchForms);
}