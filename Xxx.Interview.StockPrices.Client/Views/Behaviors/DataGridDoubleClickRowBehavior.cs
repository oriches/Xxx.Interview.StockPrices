using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace Xxx.Interview.StockPrices.Client.Views.Behaviors;

public sealed class DataGridDoubleClickRowBehavior : Behavior<DataGrid>
{
    public static readonly DependencyProperty SelectedRowProperty = DependencyProperty.Register("SelectedRow",
        typeof(object), typeof(DataGridDoubleClickRowBehavior), new PropertyMetadata(default(object)));

    public object SelectedRow
    {
        get => GetValue(SelectedRowProperty);
        set => SetValue(SelectedRowProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.Loaded += HandleLoaded;
        AssociatedObject.Unloaded += HandleUnloaded;
    }

    private void HandleLoaded(object sender, RoutedEventArgs e)
    {
        AssociatedObject.LoadingRow += HandleLoadingRow;
        AssociatedObject.UnloadingRow += HandleUnloadingRow;
    }

    private void HandleUnloaded(object sender, RoutedEventArgs e)
    {
        AssociatedObject.LoadingRow -= HandleLoadingRow;
        AssociatedObject.UnloadingRow -= HandleUnloadingRow;
    }

    private void HandleLoadingRow(object sender, DataGridRowEventArgs e)
    {
        e.Row.MouseDoubleClick += HandleMouseDoubleClick;
    }

    private void HandleUnloadingRow(object sender, DataGridRowEventArgs e)
    {
        e.Row.MouseDoubleClick -= HandleMouseDoubleClick;
    }

    private void HandleMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is DataGridRow row)
        {
            SelectedRow = row.DataContext;
            e.Handled = true;
        }
        else
        {
            SelectedRow = null;
        }
    }
}