using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Xxx.Interview.StockPrices.Service;

public abstract class DisposableObject : IDisposable
{
    private readonly CompositeDisposable _disposable;
    private readonly BehaviorSubject<bool> _isDisposed;

    protected DisposableObject()
    {
        _disposable = new CompositeDisposable();
        _isDisposed = new BehaviorSubject<bool>(false);
    }

    protected IObservable<bool> IsDisposed => _isDisposed.Where(x => x);

    public virtual void Dispose()
    {
        if (_disposable.IsDisposed)
            return;

        _disposable.Dispose();
        _isDisposed.OnNext(true);
    }

    public static implicit operator CompositeDisposable(DisposableObject disposable) => disposable._disposable;
}