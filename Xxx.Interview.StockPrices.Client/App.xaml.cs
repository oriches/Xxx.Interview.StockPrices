using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using Autofac.Core;
using NLog;
using Xxx.Interview.StockPrices.Client.Services;
using Xxx.Interview.StockPrices.Client.ViewModels;
using Xxx.Interview.StockPrices.Client.Views;
using ObservableExtensions = Xxx.Interview.StockPrices.Client.Extensions.ObservableExtensions;

namespace Xxx.Interview.StockPrices.Client;

public partial class App : Application
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private readonly CompositeDisposable _disposable;
    private IMessageService _messageService;

    private ISchedulerService _schedulerService;

    public App()
    {
        AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
        Current.DispatcherUnhandledException += DispatcherOnUnhandledException;
        TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

        _disposable = new CompositeDisposable();
    }

    protected override void OnStartup(StartupEventArgs args)
    {
        Logger.Info("Hello!");

        // ReSharper disable once RedundantToStringCallForValueType
        base.OnStartup(args);

        Bootstrapper.Start();

        _schedulerService = Bootstrapper.Resolve<ISchedulerService>();
        _messageService = Bootstrapper.Resolve<IMessageService>();

        var gestureService = Bootstrapper.Resolve<IGestureService>();
        ObservableExtensions.GestureService = gestureService;

        var window = new MainWindow(_messageService, _schedulerService);

        window.DataContext = Bootstrapper.RootVisual;

        window.Closed += HandleClosed;
        Current.Exit += HandleExit;

        // Let's go...
        window.Show();
    }

    private void HandleClosed(object sender, EventArgs e)
    {
        _disposable.Dispose();
        Bootstrapper.Stop();
    }

    private static void HandleExit(object sender, ExitEventArgs e)
    {
        Logger.Info("Bye Bye!");
        LogManager.Flush();
    }

    private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs args)
    {
        Logger.Info("Unhandled app domain exception");
        HandleException(args.ExceptionObject as Exception);
    }

    private void DispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
    {
        Logger.Info("Unhandled dispatcher thread exception");
        args.Handled = true;
        HandleException(args.Exception);
    }

    private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs args)
    {
        Logger.Info("Unhandled task exception");
        args.SetObserved();
        HandleException(args.Exception.GetBaseException());
    }

    private void HandleException(Exception exception)
    {
        Logger.Error(exception);

        _schedulerService.Dispatcher.Schedule(() =>
        {
            var parameters = new Parameter[] { new NamedParameter("exception", exception) };
            var viewModel = Bootstrapper.Resolve<IExceptionViewModel>(parameters);

            viewModel.Closed
                .Take(1)
                .Subscribe(x => viewModel.Dispose());

            _messageService.Post("Whoops - something's gone wrong!", viewModel);
        });
    }
}