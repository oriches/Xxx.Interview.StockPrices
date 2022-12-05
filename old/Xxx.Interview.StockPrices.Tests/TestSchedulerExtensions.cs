using System;
using Microsoft.Reactive.Testing;

namespace Cibc.StockPrices.Tests
{
    public static class TestSchedulerExtensions
    {
        public static void AdvanceBy(this TestScheduler testScheduler, TimeSpan timeSpan)
        {
            testScheduler.AdvanceBy(timeSpan.Ticks);
        }
    }
}