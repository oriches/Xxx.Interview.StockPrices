using Microsoft.Reactive.Testing;

namespace Xxx.Interview.StockPrices.Tests;

public static class TestSchedulerExtensions
{
    public static void AdvanceBy(this TestScheduler testScheduler, TimeSpan timeSpan) =>
        testScheduler.AdvanceBy(timeSpan.Ticks);
}