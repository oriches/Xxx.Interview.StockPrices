using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Xxx.Interview.StockPrices.Client.Views.Converters;

public sealed class SelectedStockToVisibilityConverter : IValueConverter
{
    private static readonly object Collapsed = Visibility.Collapsed;
    private static readonly object Visible = Visibility.Visible;

    public static IValueConverter Instance = new SelectedStockToVisibilityConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value == null ? Collapsed : Visible;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        Binding.DoNothing;
}