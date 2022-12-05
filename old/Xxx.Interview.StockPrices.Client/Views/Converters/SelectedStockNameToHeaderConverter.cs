using System;
using System.Globalization;
using System.Windows.Data;

namespace Cibc.StockPrices.Client.Views.Converters
{
    public sealed class SelectedStockNameToHeaderConverter : IValueConverter
    {
        private static readonly object DefaultHeader = "HISTORY";

        public static IValueConverter Instance = new SelectedStockNameToHeaderConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stockName) return $"HISTORY - {stockName}";

            return DefaultHeader;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }
}