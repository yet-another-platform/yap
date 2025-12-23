using Client.Interfaces;
using ReactiveUI;

namespace Client.ViewModels;

public class ViewModelBase<T> : ReactiveObject, IViewModel where T : ViewModelBase<T>
{
    public string ViewModelKey { get; } =  typeof(T).Name;
}