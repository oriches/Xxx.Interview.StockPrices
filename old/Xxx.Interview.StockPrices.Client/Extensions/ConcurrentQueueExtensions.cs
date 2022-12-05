using System.Collections.Concurrent;
using System.Linq;

namespace Cibc.StockPrices.Client.Extensions
{
    public static class ConcurrentQueueExtensions
    {
        public static void Clear<T>(this ConcurrentQueue<T> queue)
        {
            while (queue.Any()) queue.TryDequeue(out _);
        }
    }
}