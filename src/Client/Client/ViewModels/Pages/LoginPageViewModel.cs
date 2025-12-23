using Client.Interfaces;
using Client.ViewModels.Controls;
using ReactiveUI;

namespace Client.ViewModels.Pages;

public class LoginPageViewModel : ViewModelBase<LoginPageViewModel>
{
    private LoginFormViewModel LoginFormViewModel => new(this);
    private RegisterFormViewModel RegisterFormViewModel => new(this);
    private IViewModel _form;

    public IViewModel Form
    {
        get => _form;
        set => this.RaiseAndSetIfChanged(ref _form, value);
    }

    public LoginPageViewModel()
    {
        _form = LoginFormViewModel;
    }

    public void SwitchForms()
    {
        if (Form is LoginFormViewModel)
        {
            Form = RegisterFormViewModel;
        }
        else
        {
            Form = LoginFormViewModel;
        }
    }
}