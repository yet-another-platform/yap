using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Client.Models.Interfaces;
using Client.Net;
using Client.ViewModels.Pages;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;

namespace Client.ViewModels.Controls;

public class LoginFormViewModel(IAuthSession auth) : ViewModelBase<LoginFormViewModel>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    private string _resultText = string.Empty;
    public string ResultText
    {
        get => _resultText;
        set => this.RaiseAndSetIfChanged(ref _resultText, value);
    }
    
    public ICommand SwitchFormsCommand => new Command(_loginPageViewModel.SwitchForms);
    public IAsyncRelayCommand LoginCommand => new AsyncRelayCommand(LoginAsync);

    private LoginPageViewModel _loginPageViewModel = null!;

    public LoginFormViewModel WithParent(LoginPageViewModel loginPageViewModel)
    {
        _loginPageViewModel = loginPageViewModel;
        return this;
    }

    private async Task LoginAsync()
    {
        await auth.LoginAsync(Username, Password);
        if (!auth.IsLoggedIn)
        {
            ResultText = "Failed to login";
            return;
        }

        ResultText = $"You are now logged in as\n{auth.CurrentUser!.Username}";
    }
}