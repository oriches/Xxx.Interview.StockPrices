using System;

namespace Cibc.StockPrices.Client.ViewModels
{
    public interface ICloseableViewModel : IViewModel
    {
        IObservable<bool> Closed { get; }
    }
}