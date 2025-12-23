using System.Windows.Input;
using Client.ViewModels.Pages;

namespace Client.ViewModels.Controls;

public class RegisterFormViewModel(LoginPageViewModel loginPageViewModel) : ViewModelBase<RegisterFormViewModel>
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public ICommand SwitchFormsCommand { get; } = new Command(loginPageViewModel.SwitchForms);
}