using Client.ViewModels;

namespace Client.Interfaces;

public interface INavigationService
{
    public void GoToPage<TViewModel>() where TViewModel : ViewModelBase<TViewModel>;
}