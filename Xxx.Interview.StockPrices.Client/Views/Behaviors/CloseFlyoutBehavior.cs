using System.Linq;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Microsoft.Xaml.Behaviors;
using Microsoft.Xaml.Behaviors.Core;

namespace Xxx.Interview.StockPrices.Client.Views.Behaviors;

public sealed class CloseFlyoutBehavior : Behavior<Flyout>
{
    public static readonly DependencyProperty SelectedRowProperty = DependencyProperty.Register("SelectedRow",
        typeof(object), typeof(CloseFlyoutBehavior), new PropertyMetadata(default(object)));

    private ActionCommand _command;

    private KeyBinding _escapeKeyBinding;
    private MetroWindow _window;

    public object SelectedRow
    {
        get => GetValue(SelectedRowProperty);
        set => SetValue(SelectedRowProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        _command = new ActionCommand(() => { SelectedRow = null; });
        _escapeKeyBinding = new KeyBinding(_command, Key.Escape, ModifierKeys.None);

        AssociatedObject.CloseCommand = _command;
        AssociatedObject.Loaded += HandleLoaded;
    }

    private void HandleLoaded(object sender, RoutedEventArgs e)
    {
        if (_window == null)
            _window = (MetroWindow)AssociatedObject
                .GetAncestors()
                .First(t => t is MetroWindow);

        _window.InputBindings.Add(_escapeKeyBinding);
    }
}