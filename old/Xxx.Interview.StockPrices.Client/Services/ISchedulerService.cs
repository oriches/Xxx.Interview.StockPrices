using System.Reactive.Concurrency;

namespace Cibc.StockPrices.Client.Services
{
    public interface ISchedulerService
    {
        IScheduler Dispatcher { get; }

        IScheduler Current { get; }

        IScheduler TaskPool { get; }

        IScheduler EventLoop { get; }

        IScheduler NewThread { get; }

        IScheduler StaThread { get; }
    }
}