using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Client.Exceptions;
using Client.Extensions;
using Client.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Client;

public class ViewLocator : IDataTemplate
{
    private readonly ReadOnlyDictionary<string, Func<Control>> _knownViews;

    public ViewLocator(IServiceProvider serviceProvider)
    {
        var viewModels = serviceProvider.GetServices<IViewModel>().DistinctBy(vm => vm.ViewModelKey);
        _knownViews = viewModels.ToDictionary<IViewModel, string, Func<Control>>(
            vm => vm.ViewModelKey.Replace("ViewModel", "View", StringComparison.Ordinal),
            vm => () => (Control)serviceProvider.GetViewForViewModel(vm.ViewModelKey)).AsReadOnly();
    }

    public Control? Build(object? data)
    {
        if (data == null)
        {
            return null;
        }

        string name = data.GetType().Name.Replace("ViewModel", "View", StringComparison.Ordinal);
        if (!_knownViews.TryGetValue(name, out var factory))
        {
            throw new UnknownViewException(name);
        }

        var control = factory();
        control.DataContext = data;
        return control;
    }

    public bool Match(object? data)
    {
        return data is IViewModel;
    }
}