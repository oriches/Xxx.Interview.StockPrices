﻿using System;
using System.Reactive;
using System.Reactive.Linq;
using Cibc.StockPrices.Client.Commands;
using Cibc.StockPrices.Client.Services;

namespace Cibc.StockPrices.Client.Extensions
{
    public static class ObservableExtensions
    {
        public static IGestureService GestureService;

        public static IObservable<Unit> AsUnit<T>(this IObservable<T> observable)
        {
            return observable.Select(x => Unit.Default);
        }

        public static ReactiveCommand<object> ToCommand(this IObservable<bool> canExecute) => ReactiveCommand.Create(canExecute);

        public static IObservable<T> ActivateGestures<T>(this IObservable<T> observable)
        {
            if (GestureService == null) throw new Exception("GestureService has not been initialised");

            return observable.Do(x => GestureService.SetBusy());
        }
    }
}