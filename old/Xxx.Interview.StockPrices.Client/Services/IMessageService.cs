using System;
using Cibc.StockPrices.Client.Models;
using Cibc.StockPrices.Client.ViewModels;

namespace Cibc.StockPrices.Client.Services
{
    public interface IMessageService
    {
        IObservable<Message> Show { get; }

        void Post(string header, ICloseableViewModel viewModel);
    }
}