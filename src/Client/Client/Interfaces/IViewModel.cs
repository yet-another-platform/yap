using ReactiveUI;

namespace Client.Interfaces;

public interface IViewModel : IReactiveObject
{
    public string ViewModelKey { get; }
}