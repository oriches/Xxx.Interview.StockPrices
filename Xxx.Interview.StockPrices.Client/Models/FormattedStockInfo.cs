using System;
using System.Diagnostics;
using Xxx.Interview.StockPrices.Service;

namespace Xxx.Interview.StockPrices.Client.Models;

[DebuggerDisplay("Name=[{Name}], Price=[{PriceAsString}]")]
public readonly struct FormattedStockInfo : IEquatable<FormattedStockInfo>
{
    public static readonly FormattedStockInfo Empty = new();

    public FormattedStockInfo(StockInfo stockInfo)
    {
        Name = string.Intern(stockInfo.Name.ToUpper());

        Timestamp = stockInfo.Timestamp;
        TimestampAsString = stockInfo.Timestamp.ToString("F");

        Price = stockInfo.Price;
        PriceAsString = stockInfo.Price.ToString("##.00");
    }

    public string PriceAsString { get; }

    public string Name { get; }

    public DateTimeOffset Timestamp { get; }

    public string TimestampAsString { get; }

    public decimal Price { get; }

    public bool Equals(FormattedStockInfo other) => Name == other.Name;

    public override bool Equals(object obj) => obj is FormattedStockInfo other && Equals(other);

    public override int GetHashCode() => Name.GetHashCode();

    public static bool operator ==(FormattedStockInfo left, FormattedStockInfo right) => Equals(left, right);

    public static bool operator !=(FormattedStockInfo left, FormattedStockInfo right) => !Equals(left, right);
}