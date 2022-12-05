using System.Reactive.Disposables;
using System.Security.Cryptography;
using NLog;
using Xxx.Interview.StockPrices.Core.Extensions;

namespace Xxx.Interview.StockPrices.Service;

public sealed class CryptoRandomStockProvider : BaseRandomStockProvider
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public CryptoRandomStockProvider()
    {
        Disposable.Create(() => Logger.Info("Shutting down"))
            .DisposeWith(this);
    }

    public override string Name => "Crypto Random Prices";

    protected override int Next(int min, int max)
    {
        if (min > max) throw new ArgumentOutOfRangeException(nameof(min));
        if (min == max) return min;

        var data = RandomNumberGenerator.GetBytes(4);

        var generatedValue = Math.Abs(BitConverter.ToInt32(data, 0));

        var diff = max - min;
        var mod = generatedValue % diff;
        var normalizedNumber = min + mod;

        return normalizedNumber;
    }
}