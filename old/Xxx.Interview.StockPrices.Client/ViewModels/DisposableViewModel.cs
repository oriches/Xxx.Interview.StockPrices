using System.Reactive.Disposables;

namespace Cibc.StockPrices.Client.ViewModels
{
    public abstract class DisposableViewModel : BaseViewModel, IDisposableViewModel
    {
        private readonly CompositeDisposable _disposable;

        protected DisposableViewModel() => _disposable = new CompositeDisposable();

        public void Dispose()
        {
            _disposable.Dispose();
        }

        public static implicit operator CompositeDisposable(DisposableViewModel disposable) => disposable._disposable;
    }
}