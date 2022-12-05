using System;
using Xxx.Interview.StockPrices.Client.Models;
using Xxx.Interview.StockPrices.Client.ViewModels;

namespace Xxx.Interview.StockPrices.Client.Services;

public interface IMessageService
{
    IObservable<Message> Show { get; }

    void Post(string header, ICloseableViewModel viewModel);
}