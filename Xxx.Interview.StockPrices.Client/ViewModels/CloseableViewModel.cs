using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Xxx.Interview.StockPrices.Core.Extensions;

namespace Xxx.Interview.StockPrices.Client.ViewModels;

public abstract class CloseableViewModel : DisposableViewModel, ICloseableViewModel
{
    private readonly BehaviorSubject<bool> _closed;

    protected CloseableViewModel() =>
        _closed = new BehaviorSubject<bool>(false)
            .DisposeWith(this);

    public IObservable<bool> Closed => _closed.Where(x => x)
        .DistinctUntilChanged();

    protected void Close() => _closed.OnNext(true);
}