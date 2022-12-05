using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;

namespace Cibc.StockPrices.Service
{
    public interface IStockProviderFactory
    {
        IObservable<IEnumerable<IStockProvider>> Scan();

        IObservable<IEnumerable<IStockProvider>> Scan(IScheduler scheduler);
    }
}