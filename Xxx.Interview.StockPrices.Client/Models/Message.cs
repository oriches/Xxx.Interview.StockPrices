﻿using Xxx.Interview.StockPrices.Client.ViewModels;

namespace Xxx.Interview.StockPrices.Client.Models;

public sealed class Message
{
    public Message(string header, ICloseableViewModel viewModel)
    {
        Header = header;
        ViewModel = viewModel;
    }

    public string Header { get; }

    public ICloseableViewModel ViewModel { get; }
}