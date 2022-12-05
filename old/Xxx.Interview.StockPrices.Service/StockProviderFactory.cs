using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reflection;
using NLog;

namespace Cibc.StockPrices.Service
{
    public sealed class StockProviderFactory : IStockProviderFactory
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public IObservable<IEnumerable<IStockProvider>> Scan() => Scan(TaskPoolScheduler.Default);

        public IObservable<IEnumerable<IStockProvider>> Scan(IScheduler scheduler) => Observable.Start(ScanImpl, scheduler);

        private static IEnumerable<IStockProvider> ScanImpl()
        {
            var result = new List<IStockProvider>();

            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                .Location);

            if (directory == null)
                return result;

            var files = Directory.GetFiles(directory, "Cibc*.dll");
            foreach (var file in files)
            {
                var providers = Assembly.LoadFile(file)
                    .GetTypes()
                    .Where(t => !t.IsAbstract && t.IsClass && t.IsPublic && t.GetInterfaces()
                        .Contains(typeof(IStockProvider)))
                    .Select(Create)
                    .ToArray();

                if (providers.Any())
                    result.AddRange(providers);
            }

            Logger.Info($"Found {result.Count} IStockProvider implmentations");

            return result;
        }

        private static IStockProvider Create(Type type)
        {
            Logger.Info($"Creating StockProvider, Type=[{type.FullName}]");

            // simple creation because examples all have parameter-less constructors
            var stockProvider = (IStockProvider) Activator.CreateInstance(type);

            return stockProvider;
        }
    }
}