using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Xxx.Interview.StockPrices.Client.Services;

namespace Xxx.Interview.StockPrices.Client.Views;

public partial class MainWindow : MetroWindow
{
    private static readonly TimeSpan ExceptionDelay = TimeSpan.FromMilliseconds(250);

    private readonly IDisposable _disposable;

    public MainWindow(IMessageService messageService, ISchedulerService schedulerService)
    {
        InitializeComponent();

        _disposable = messageService.Show
            .Where(x => x != null)
            // Delay to make sure there is time for the animations
            .Delay(ExceptionDelay, schedulerService.Dispatcher)
            .Select(x => new MessageDialog(x))
            .SelectMany(ShowDialogAsync, (x, y) => x)
            .Subscribe();

        Closed += HandleClosed;
    }

    private void HandleClosed(object sender, EventArgs e) => _disposable.Dispose();

    private IObservable<Unit> ShowDialogAsync(MessageDialog dialog)
    {
        var settings = new MetroDialogSettings
        {
            AnimateShow = true,
            AnimateHide = true,
            ColorScheme = MetroDialogColorScheme.Accented
        };

        return this.ShowMetroDialogAsync(dialog, settings)
            .ToObservable()
            .SelectMany(x => dialog.CloseableContent.Closed, (x, y) => x)
            .SelectMany(x => this.HideMetroDialogAsync(dialog)
                .ToObservable(), (x, y) => x);
    }
}