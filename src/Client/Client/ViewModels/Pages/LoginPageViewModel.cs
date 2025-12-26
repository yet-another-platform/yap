using System;
using System.Threading.Tasks;
using Client.Interfaces;
using Client.ViewModels.Controls;
using Client.Views;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Client.ViewModels.Pages;

public class LoginPageViewModel : ViewModelBase<LoginPageViewModel>
{
    private readonly IServiceProvider _serviceProvider;
    private LoginFormViewModel LoginFormViewModel => _serviceProvider.GetRequiredService<LoginFormViewModel>().WithParent(this);
    private RegisterFormViewModel RegisterFormViewModel => new(this);
    private IViewModel _form;

    public IViewModel Form
    {
        get => _form;
        set => this.RaiseAndSetIfChanged(ref _form, value);
    }

    public LoginPageViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
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