using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.Loader;
using NLog;

namespace Xxx.Interview.StockPrices.Service;

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

        var files = Directory.GetFiles(directory, "Xxx.Interview.*.dll");

        foreach (var file in files)
        {
            var types = AssemblyLoadContext.Default.LoadFromAssemblyPath(file)
                .GetTypes()
                .Where(type => type.IsAssignableTo(typeof(IStockProvider)) && !type.IsAbstract)
                .ToArray();

            var providers = types.Select(Create)
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
        var stockProvider = (IStockProvider)Activator.CreateInstance(type);

        return stockProvider;
    }
}