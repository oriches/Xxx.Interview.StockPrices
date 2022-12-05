using System;
using System.Reactive.Disposables;
using Cibc.StockPrices.Core.Extensions;
using NLog;

namespace Cibc.StockPrices.Service
{
    public sealed class PseudoRandomStockProvider : BaseRandomStockProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Random _random;

        public PseudoRandomStockProvider()
        {
            _random = new Random(42);

            Disposable.Create(() => Logger.Info("Shutting down"))
                .DisposeWith(this);
        }

        public override string Name => "Pseudo Random Prices";

        protected override int Next(int min, int max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException(nameof(min));

            return min == max ? min : _random.Next(min, max);
        }
    }
}