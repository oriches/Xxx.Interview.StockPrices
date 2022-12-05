using Xxx.Interview.StockPrices.Client.Commands;

namespace Xxx.Interview.StockPrices.Client.ViewModels;

public interface IExceptionViewModel : ICloseableViewModel, IDisposableViewModel
{
    ReactiveCommand<object> OpenLogFolderCommand { get; }

    ReactiveCommand<object> CopyCommand { get; }

    ReactiveCommand<object> ContinueCommand { get; }

    ReactiveCommand<object> ExitCommand { get; }

    ReactiveCommand<object> RestartCommand { get; }

    string Message { get; }
}