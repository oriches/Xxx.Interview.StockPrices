using NUnit.Framework;
using Xxx.Interview.StockPrices.Service;

namespace Xxx.Interview.StockPrices.Tests.Service;

[TestFixture]
public sealed class CryptoRandomStockProviderTests : BaseTests
{
    [Test]
    public void generates_values_in_expect_range_for_stock_1()
    {
        // ARRANGE
        var provider = new CryptoRandomStockProvider();

        var results = new List<StockInfo>();

        // ACT
        provider.Subscribe(Constants.Stocks.Stock1, Scheduler)
            .Subscribe(pi => results.Add(pi));

        Scheduler.AdvanceBy(TimeSpan.FromSeconds(100));

        // ASSERT
        Assert.That(results.Count, Is.EqualTo(100));
        Assert.That(results.All(pi => pi.Price >= 240), Is.True);
        Assert.That(results.All(pi => pi.Price <= 270), Is.True);
    }

    [Test]
    public void generates_values_in_expect_range_for_stock_2()
    {
        // ARRANGE
        var provider = new CryptoRandomStockProvider();

        var results = new List<StockInfo>();

        // ACT
        provider.Subscribe(Constants.Stocks.Stock2, Scheduler)
            .Subscribe(pi => results.Add(pi));

        Scheduler.AdvanceBy(TimeSpan.FromSeconds(1000));

        // ASSERT
        Assert.That(results.Count, Is.EqualTo(1000));
        Assert.That(results.All(pi => pi.Price >= 180), Is.True);
        Assert.That(results.All(pi => pi.Price <= 210), Is.True);
    }

    [Test]
    public void does_not_generate_price_after_subscription_is_disposed()
    {
        // ARRANGE
        var provider = new CryptoRandomStockProvider();

        var results = new List<StockInfo>();

        // ACT
        var disposed = provider.Subscribe(Constants.Stocks.Stock2, Scheduler)
            .Subscribe(pi => results.Add(pi));

        Scheduler.AdvanceBy(TimeSpan.FromSeconds(1));

        disposed.Dispose();

        Scheduler.AdvanceBy(TimeSpan.FromSeconds(1));

        // ASSERT
        Assert.That(results.Count, Is.EqualTo(1));
    }

    [Test]
    public void does_not_generate_price_after_provider_is_disposed()
    {
        // ARRANGE
        var provider = new CryptoRandomStockProvider();

        var results = new List<StockInfo>();

        // ACT
        var disposed = provider.Subscribe(Constants.Stocks.Stock2, Scheduler)
            .Subscribe(pi => results.Add(pi));

        Scheduler.AdvanceBy(TimeSpan.FromSeconds(1));

        provider.Dispose();

        Scheduler.AdvanceBy(TimeSpan.FromSeconds(200));

        // ASSERT
        Assert.That(results.Count, Is.EqualTo(1));
    }
}