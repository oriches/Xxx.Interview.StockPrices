using System.Reactive.Concurrency;

namespace Xxx.Interview.StockPrices.Service;

public interface IStockProviderFactory
{
    IObservable<IEnumerable<IStockProvider>> Scan();

    IObservable<IEnumerable<IStockProvider>> Scan(IScheduler scheduler);
}