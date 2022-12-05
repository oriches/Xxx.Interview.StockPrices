using System;
using System.ComponentModel;

namespace Cibc.StockPrices.Client.ViewModels
{
    public interface IDisposableViewModel : INotifyPropertyChanged, IDisposable
    {
    }
}