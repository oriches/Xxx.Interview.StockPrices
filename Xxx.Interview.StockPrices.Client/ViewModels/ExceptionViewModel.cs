using System;
using System.Reactive.Linq;
using Xxx.Interview.StockPrices.Client.Commands;
using Xxx.Interview.StockPrices.Client.Services;
using Xxx.Interview.StockPrices.Core.Extensions;

namespace Xxx.Interview.StockPrices.Client.ViewModels;

public sealed class ExceptionViewModel : CloseableViewModel, IExceptionViewModel
{
    private readonly IApplicationService _applicationService;
    private readonly Exception _exception;

    public ExceptionViewModel(Exception exception, IApplicationService applicationService)
    {
        _exception = exception;
        _applicationService = applicationService;

        OpenLogFolderCommand = ReactiveCommand.Create(Observable.Return(_applicationService.LogFolder != null))
            .DisposeWith(this);

        CopyCommand = ReactiveCommand.Create(Observable.Return(exception != null))
            .DisposeWith(this);

        ContinueCommand = ReactiveCommand.Create()
            .DisposeWith(this);

        ExitCommand = ReactiveCommand.Create()
            .DisposeWith(this);

        RestartCommand = ReactiveCommand.Create()
            .DisposeWith(this);

        OpenLogFolderCommand.Subscribe(x => OpenLogFolder())
            .DisposeWith(this);

        CopyCommand.Subscribe(x => Copy())
            .DisposeWith(this);

        ContinueCommand.Subscribe(x => Continue())
            .DisposeWith(this);

        ExitCommand.Subscribe(x => Exit())
            .DisposeWith(this);

        RestartCommand.Subscribe(x => Restart())
            .DisposeWith(this);

        Closed.Take(1)
            .Subscribe(x =>
            {
                // Force all other potential exceptions to be realized
                // from the finalizer thread to surface to the UI

                GC.Collect(2, GCCollectionMode.Forced);
                GC.WaitForPendingFinalizers();
            })
            .DisposeWith(this);
    }

    public ReactiveCommand<object> CopyCommand { get; }

    public ReactiveCommand<object> OpenLogFolderCommand { get; }

    public ReactiveCommand<object> ContinueCommand { get; }

    public ReactiveCommand<object> ExitCommand { get; }

    public ReactiveCommand<object> RestartCommand { get; }

    public string Message => _exception?.Message;

    private void Copy()
    {
        _applicationService.CopyToClipboard(_exception.ToString());
    }

    private void Exit()
    {
        _applicationService.Exit();
    }

    private void Restart()
    {
        _applicationService.Restart();
    }

    private void Continue()
    {
        Close();
    }

    private void OpenLogFolder()
    {
        _applicationService.OpenFolder(_applicationService.LogFolder);
    }
}