using System.Reactive.Disposables;
using NLog;
using Xxx.Interview.StockPrices.Core.Extensions;

namespace Xxx.Interview.StockPrices.Service;

public sealed class RNGCryptoServiceProvider : BaseRandomStockProvider
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private readonly System.Security.Cryptography.RNGCryptoServiceProvider _random;

    public RNGCryptoServiceProvider()
    {
        _random = new System.Security.Cryptography.RNGCryptoServiceProvider().DisposeWith(this);

        Disposable.Create(() => Logger.Info("Shutting down"))
            .DisposeWith(this);
    }

    public override string Name => "RNG Crypto Random Prices";

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