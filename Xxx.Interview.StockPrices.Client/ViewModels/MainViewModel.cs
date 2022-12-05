using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using NLog;
using Xxx.Interview.StockPrices.Client.Commands;
using Xxx.Interview.StockPrices.Client.Extensions;
using Xxx.Interview.StockPrices.Client.Models;
using Xxx.Interview.StockPrices.Client.Services;
using Xxx.Interview.StockPrices.Core.Extensions;
using Xxx.Interview.StockPrices.Service;

namespace Xxx.Interview.StockPrices.Client.ViewModels;

public sealed class MainViewModel : DisposableViewModel, IMainViewModel
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private readonly IStockProviderFactory _factory;
    private readonly ISchedulerService _schedulerService;
    private readonly ObservableCollection<IStockProvider> _stockProviders;
    private readonly ObservableCollection<StockPriceViewModel> _stocks;
    private readonly Dictionary<string, StockPriceViewModel> _stocksDictionary;
    private readonly SerialDisposable _stocksDisposable;

    private IStockProvider _selectedProvider;
    private StockPriceViewModel _selectedStock;

    public MainViewModel(IStockProviderFactory factory, IApplicationService applicationService,
        ISchedulerService schedulerService)
    {
        _factory = factory;
        _schedulerService = schedulerService;
        _stocksDisposable = new SerialDisposable().DisposeWith(this);

        _stockProviders = new ObservableCollection<IStockProvider>();
        _stocks = new ObservableCollection<StockPriceViewModel>();
        _stocksDictionary = new Dictionary<string, StockPriceViewModel>();

        ClearCommand = ReactiveCommand.Create(CanClear())
            .DisposeWith(this);

        ScanCommand = ReactiveCommand.Create()
            .DisposeWith(this);

        OpenLogFolderCommand = ReactiveCommand.Create()
            .DisposeWith(this);

        ClearCommand.Subscribe(x =>
            {
                SelectedStock = null;
                SelectedProvider = null;
            })
            .DisposeWith(this);

        ScanCommand.Subscribe(x => ScanImpl())
            .DisposeWith(this);

        OpenLogFolderCommand.Subscribe(x => applicationService.OpenFolder(applicationService.LogFolder))
            .DisposeWith(this);

        Disposable.Create(ClearProviders)
            .DisposeWith(this);

        ScanCommand.Execute(null);
    }

    public ReactiveCommand<object> OpenLogFolderCommand { get; }

    public ReactiveCommand<object> ScanCommand { get; }

    public ReactiveCommand<object> ClearCommand { get; }

    public IEnumerable<IStockProvider> StockProviders => _stockProviders;

    public IEnumerable<StockPriceViewModel> Stocks => _stocks;

    public IStockProvider SelectedProvider
    {
        get => _selectedProvider;
        set
        {
            if (SetProperty(ref _selectedProvider, value))
            {
                SelectedStock = null;
                SubscribeForPrices();
            }
        }
    }

    public StockPriceViewModel SelectedStock
    {
        get => _selectedStock;
        set => SetProperty(ref _selectedStock, value);
    }

    private void ScanImpl()
    {
        SelectedProvider = null;
        SelectedStock = null;

        ClearProviders();

        _factory.Scan(_schedulerService.TaskPool)
            .Select(sps => sps.OrderBy(sp => sp.Name))
            .ObserveOn(_schedulerService.Dispatcher)
            .ActivateGestures()
            .Subscribe(sp =>
            {
                _stockProviders.AddRange(sp);
                SelectedProvider = _stockProviders.FirstOrDefault();
            })
            .DisposeWith(this);
    }

    private IObservable<bool> CanClear()
    {
        return this.ObservePropertyChanged(nameof(SelectedProvider))
            .AsUnit()
            .StartWith(Unit.Default)
            .Select(x => SelectedProvider != null)
            .DistinctUntilChanged();
    }

    private void ClearProviders()
    {
        if (_stockProviders == null)
            return;

        var local = _stockProviders.ToArray();
        _stockProviders.Clear();

        local.ForEach(x => x.Dispose());
    }

    private void SubscribeForPrices()
    {
        _stocksDisposable.Disposable = Disposable.Empty;

        _stocks.Clear();
        _stocksDictionary.Clear();

        if (_selectedProvider == null)
            return;

        Logger.Info($"SelectedProvider Name=[{_selectedProvider.Name}], Type=[{_selectedProvider.GetType()}]");

        _stocksDisposable.Disposable = _selectedProvider.AvailableStocks
            .Select(s => _selectedProvider.Subscribe(s, _schedulerService.TaskPool))
            .Merge()
            .Select(si => new FormattedStockInfo(si))
            .ObserveOn(_schedulerService.Dispatcher)
            .Subscribe(fsi =>
            {
                if (!_stocksDictionary.TryGetValue(fsi.Name, out var viewModel))
                {
                    viewModel = new StockPriceViewModel(fsi);
                    _stocks.Add(viewModel);
                    _stocksDictionary.Add(fsi.Name, viewModel);
                }
                else
                {
                    viewModel.Update(fsi);
                }
            });
    }
}