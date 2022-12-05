using NUnit.Framework;
using Xxx.Interview.StockPrices.Service;

// ReSharper disable UnusedVariable

namespace Xxx.Interview.StockPrices.Tests.Service;

[TestFixture]
public sealed class StockProviderFactoryTests : BaseTests
{
    [Test]
    public void scans_for_interface_implementations()
    {
        // ARRANGE
        var factory = new StockProviderFactory();

        var providers = new List<IStockProvider>();

        // ACT
        factory.Scan(Scheduler)
            .Subscribe(x => providers.AddRange(x));

        Scheduler.AdvanceBy(TimeSpan.FromSeconds(10));

        // ASSERT
        Assert.That(providers.Count, Is.EqualTo(3));
    }

    [Test]
    public void ticks_with_prices()
    {
        // ARRANGE
        IStockProvider provider = null;

        var results = new List<StockInfo>();

        // ACT
        new StockProviderFactory()
            .Scan(Scheduler)
            .Subscribe(x => provider = x.First());

        Scheduler.AdvanceBy(TimeSpan.FromSeconds(1));

        provider.Subscribe(Constants.Stocks.Stock1, Scheduler)
            .Subscribe(pi => results.Add(pi));

        Scheduler.AdvanceBy(TimeSpan.FromSeconds(10));

        // ASSERT
        Assert.That(results.Count, Is.EqualTo(10));
    }
}