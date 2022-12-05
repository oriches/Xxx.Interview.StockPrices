using System.Reactive.Concurrency;

namespace Xxx.Interview.StockPrices.Service;

public interface IStockProvider : IDisposable
{
    string Name { get; }

    IEnumerable<string> AvailableStocks { get; }

    IObservable<StockInfo> Subscribe(string stockName);

    IObservable<StockInfo> Subscribe(string stockName, IScheduler scheduler);
}