using Microsoft.Reactive.Testing;

namespace Cibc.StockPrices.Tests
{
    public abstract class BaseTests
    {
        protected TestScheduler Scheduler;

        protected BaseTests() => Scheduler = new TestScheduler();
    }
}