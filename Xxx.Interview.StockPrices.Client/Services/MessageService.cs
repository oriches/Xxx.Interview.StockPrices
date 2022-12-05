using System;
using System.Collections.Concurrent;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Xxx.Interview.StockPrices.Client.Models;
using Xxx.Interview.StockPrices.Client.ViewModels;
using Xxx.Interview.StockPrices.Core.Extensions;
using Xxx.Interview.StockPrices.Service;

namespace Xxx.Interview.StockPrices.Client.Services;

public sealed class MessageService : DisposableObject, IMessageService
{
    private readonly Subject<Message> _show;
    private readonly ConcurrentQueue<Message> _waitingMessages = new();

    public MessageService()
    {
        _show = new Subject<Message>()
            .DisposeWith(this);

        Disposable.Create(() => _waitingMessages.Clear())
            .DisposeWith(this);
    }

    public void Post(string header, ICloseableViewModel viewModel)
    {
        if (string.IsNullOrEmpty(header))
            throw new ArgumentException(@"Message Header is null or empty!", nameof(header));

        if (viewModel == null)
            throw new ArgumentException(@"Message ViewModel is null!", nameof(viewModel));

        var newMessage = new Message(header, viewModel);

        newMessage.ViewModel.Closed
            .Take(1)
            .Subscribe(x =>
            {
                _waitingMessages.TryDequeue(out _);

                if (_waitingMessages.TryPeek(out var nextMessage))
                    _show.OnNext(nextMessage);
            })
            .DisposeWith(this);

        _waitingMessages.Enqueue(newMessage);
        if (_waitingMessages.Count == 1)
            _show.OnNext(newMessage);
    }

    public IObservable<Message> Show => _show;
}