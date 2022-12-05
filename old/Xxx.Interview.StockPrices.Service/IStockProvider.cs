using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;

namespace Cibc.StockPrices.Service
{
    public interface IStockProvider : IDisposable
    {
        string Name { get; }

        IEnumerable<string> AvailableStocks { get; }

        IObservable<StockInfo> Subscribe(string stockName);

        IObservable<StockInfo> Subscribe(string stockName, IScheduler scheduler);
    }
}