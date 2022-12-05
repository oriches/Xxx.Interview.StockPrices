using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xxx.Interview.StockPrices.Client.Models;

namespace Xxx.Interview.StockPrices.Client.ViewModels;

public sealed class StockPriceViewModel : BaseViewModel, IEquatable<StockPriceViewModel>
{
    private readonly ObservableCollection<FormattedStockInfo> _history;

    private FormattedStockInfo _current;
    private Trend _trend;

    public StockPriceViewModel(FormattedStockInfo stockInfo)
    {
        _history = new ObservableCollection<FormattedStockInfo>();

        Name = stockInfo.Name;

        Update(stockInfo);
    }

    public string Name { get; }

    public decimal? Price => _current.Price;

    public string PriceAsString => _current.PriceAsString;

    public DateTimeOffset? Timestamp => _current.Timestamp;

    public string TimestampAsString => _current.TimestampAsString;

    public IEnumerable<FormattedStockInfo> History => _history;

    public Trend Trend
    {
        get => _trend;
        set => SetProperty(ref _trend, value);
    }

    public bool Equals(StockPriceViewModel other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name;
    }

    public override bool Equals(object obj) =>
        ReferenceEquals(this, obj) || (obj is StockPriceViewModel other && Equals(other));

    public override int GetHashCode() => Name.GetHashCode();

    public static bool operator ==(StockPriceViewModel left, StockPriceViewModel right) => Equals(left, right);

    public static bool operator !=(StockPriceViewModel left, StockPriceViewModel right) => !Equals(left, right);

    public void Update(FormattedStockInfo stockInfo)
    {
        if (stockInfo == FormattedStockInfo.Empty)
            throw new ArgumentNullException(nameof(stockInfo));

        _history.Add(stockInfo);

        if (_current == FormattedStockInfo.Empty)
        {
            _current = stockInfo;
            _trend = Trend.Neutral;

            RaisePropertiesChanged(nameof(Price), nameof(PriceAsString), nameof(Timestamp),
                nameof(TimestampAsString), nameof(Trend));
        }
        else
        {
            var previous = _current;
            _current = stockInfo;

            if (_current.Price > previous.Price)
            {
                RaisePropertiesChanged(nameof(Price), nameof(PriceAsString), nameof(Timestamp),
                    nameof(TimestampAsString));
                Trend = Trend.Positive;
            }
            else if (_current.Price < previous.Price)
            {
                RaisePropertiesChanged(nameof(Price), nameof(PriceAsString), nameof(Timestamp),
                    nameof(TimestampAsString));
                Trend = Trend.Negative;
            }
            else
            {
                Trend = Trend.Neutral;
                RaisePropertiesChanged(nameof(Timestamp), nameof(TimestampAsString));
            }
        }
    }
}