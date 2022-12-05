using Microsoft.Reactive.Testing;

namespace Xxx.Interview.StockPrices.Tests;

public abstract class BaseTests
{
    protected TestScheduler Scheduler;

    protected BaseTests() => Scheduler = new TestScheduler();
}