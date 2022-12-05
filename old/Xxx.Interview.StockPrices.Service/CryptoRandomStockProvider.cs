using System;
using System.Reactive.Disposables;
using System.Security.Cryptography;
using Cibc.StockPrices.Core.Extensions;
using NLog;

namespace Cibc.StockPrices.Service
{
    public sealed class CryptoRandomStockProvider : BaseRandomStockProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly RNGCryptoServiceProvider _random;

        public CryptoRandomStockProvider()
        {
            _random = new RNGCryptoServiceProvider().DisposeWith(this);

            Disposable.Create(() => Logger.Info("Shutting down"))
                .DisposeWith(this);
        }

        public override string Name => "Crypto Random Prices";

        protected override int Next(int min, int max)
        {
            if (min > max) throw new ArgumentOutOfRangeException(nameof(min));
            if (min == max) return min;

            var data = new byte[4];
            _random.GetBytes(data);

            var generatedValue = Math.Abs(BitConverter.ToInt32(data, 0));

            var diff = max - min;
            var mod = generatedValue % diff;
            var normalizedNumber = min + mod;

            return normalizedNumber;
        }
    }
}