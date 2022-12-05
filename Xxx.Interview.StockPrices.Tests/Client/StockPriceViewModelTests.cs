using NUnit.Framework;
using Xxx.Interview.StockPrices.Client.Models;
using Xxx.Interview.StockPrices.Client.ViewModels;
using Xxx.Interview.StockPrices.Service;

namespace Xxx.Interview.StockPrices.Tests.Client;

[TestFixture]
public sealed class StockPriceViewModelTests : BaseTests
{
    [Test]
    public void trend_is_neutral_for_first_price()
    {
        // ARRANGE
        var fsi = new FormattedStockInfo(new StockInfo("STOCK_1", DateTimeOffset.Now, 100.00m));

        // ACT
        var viewModel = new StockPriceViewModel(fsi);

        // ASSERT
        Assert.That(viewModel.Trend, Is.EqualTo(Trend.Neutral));
    }

    [Test]
    public void trend_is_positive()
    {
        // ARRANGE
        var fsi1 = new FormattedStockInfo(new StockInfo("STOCK_1", DateTimeOffset.Now, 100.00m));
        var fsi2 = new FormattedStockInfo(new StockInfo("STOCK_1", DateTimeOffset.Now.AddSeconds(1), 100.50m));

        var viewModel = new StockPriceViewModel(fsi1);

        // ACT
        viewModel.Update(fsi2);

        // ASSERT
        Assert.That(viewModel.Trend, Is.EqualTo(Trend.Positive));
        Assert.That(viewModel.Timestamp, Is.EqualTo(fsi2.Timestamp));
    }

    [Test]
    public void trend_is_negative()
    {
        // ARRANGE
        var fsi1 = new FormattedStockInfo(new StockInfo("STOCK_1", DateTimeOffset.Now, 100.00m));
        var fsi2 = new FormattedStockInfo(new StockInfo("STOCK_1", DateTimeOffset.Now.AddSeconds(1), 99.95m));

        var viewModel = new StockPriceViewModel(fsi1);

        // ACT
        viewModel.Update(fsi2);

        // ASSERT
        Assert.That(viewModel.Trend, Is.EqualTo(Trend.Negative));
        Assert.That(viewModel.Timestamp, Is.EqualTo(fsi2.Timestamp));
    }

    [Test]
    public void trend_is_neutral()
    {
        // ARRANGE
        var fsi1 = new FormattedStockInfo(new StockInfo("STOCK_1", DateTimeOffset.Now, 100.00m));
        var fsi2 = new FormattedStockInfo(new StockInfo("STOCK_1", DateTimeOffset.Now.AddSeconds(1), 99.95m));
        var fsi3 = new FormattedStockInfo(new StockInfo("STOCK_1", DateTimeOffset.Now.AddSeconds(2), 99.95m));

        var viewModel = new StockPriceViewModel(fsi1);

        // ACT
        viewModel.Update(fsi2);
        viewModel.Update(fsi3);

        // ASSERT
        Assert.That(viewModel.Trend, Is.EqualTo(Trend.Neutral));
        Assert.That(viewModel.Timestamp, Is.EqualTo(fsi3.Timestamp));
    }

    [Test]
    public void history_is_populated()
    {
        // ARRANGE
        var fsi1 = new FormattedStockInfo(new StockInfo("STOCK_1", DateTimeOffset.Now, 100.00m));
        var fsi2 = new FormattedStockInfo(new StockInfo("STOCK_1", DateTimeOffset.Now.AddSeconds(1), 99.95m));
        var fsi3 = new FormattedStockInfo(new StockInfo("STOCK_1", DateTimeOffset.Now.AddSeconds(2), 99.95m));

        var viewModel = new StockPriceViewModel(fsi1);

        // ACT
        viewModel.Update(fsi2);
        viewModel.Update(fsi3);

        // ASSERT
        Assert.That(viewModel.History, Contains.Item(fsi1));
        Assert.That(viewModel.History, Contains.Item(fsi2));
        Assert.That(viewModel.History, Contains.Item(fsi3));
    }
}