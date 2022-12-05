using System;

namespace Xxx.Interview.StockPrices.Client.ViewModels;

public interface ICloseableViewModel : IViewModel
{
    IObservable<bool> Closed { get; }
}