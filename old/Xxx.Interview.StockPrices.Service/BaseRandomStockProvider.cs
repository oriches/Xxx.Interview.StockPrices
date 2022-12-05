using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using NLog;

namespace Cibc.StockPrices.Service
{
    public abstract class BaseRandomStockProvider : DisposableObject, IStockProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public abstract string Name { get; }

        public IEnumerable<string> AvailableStocks => Constants.AvailableStocks;

        public IObservable<StockInfo> Subscribe(string stockName) => Subscribe(stockName, TaskPoolScheduler.Default);

        public IObservable<StockInfo> Subscribe(string stockName, IScheduler scheduler)
        {
            if (!AvailableStocks.Contains(stockName))
                throw new ArgumentException("Unknown Stock!", nameof(stockName));

            Logger.Info($"Subscribe, name=[{stockName}]");

            var range = Constants.Ranges[stockName];
            var seedPrice = SeedPrice(range.Min, range.Max);

            return Observable.Interval(Constants.Generate, scheduler)
                .TakeUntil(IsDisposed)
                .Scan(seedPrice, (currentPrice, n) => NewPrice(currentPrice, range.Min, range.Max))
                .Timestamp()
                .Select(x =>
                {
                    var (newPrice, timestamp) = x;
                    var result = new StockInfo(stockName, timestamp, Convert.ToDecimal(newPrice));

                    Logger.Info($"Name=[{result.Name}], Price=[{result.Price}]");
                    return result;
                })
                .Finally(() => { Logger.Info($"Unsubscribe, name=[{stockName}]"); });
        }

        private double NewPrice(double currentPrice, int rangeMin, int rangeMax)
        {
            while (true)
            {
                var multiplier = (double) Next(Constants.Variance.Min, Constants.Variance.Max) / 100;
                var newPrice = Math.Round(currentPrice * multiplier, 2);

                if (newPrice >= rangeMin && newPrice <= rangeMax)
                    return newPrice;
            }
        }

        private double SeedPrice(int min, int max) => (double) Next(min * 100, max * 100) / 100;

        protected abstract int Next(int min, int max);
    }
}