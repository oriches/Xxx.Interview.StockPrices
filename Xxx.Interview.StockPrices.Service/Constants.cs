namespace Xxx.Interview.StockPrices.Service;

public static class Constants
{
    public static readonly TimeSpan Generate = TimeSpan.FromMilliseconds(1000);

    public static readonly string[] AvailableStocks =
    {
        Stocks.Stock1,
        Stocks.Stock2
    };

    public static readonly Dictionary<string, Range> Ranges = new()
    {
        { Stocks.Stock1, new Range(240, 270) },
        { Stocks.Stock2, new Range(180, 210) }
    };

    public static class Stocks
    {
        public static string Stock1 = "Stock 1";
        public static string Stock2 = "Stock 2";
    }

    public static class Variance
    {
        public static int Min = 95;
        public static int Max = 105;
    }
}