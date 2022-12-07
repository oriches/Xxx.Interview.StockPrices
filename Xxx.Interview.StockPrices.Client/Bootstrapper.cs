using System;
using Autofac;
using Autofac.Core;
using Xxx.Interview.StockPrices.Client.Services;
using Xxx.Interview.StockPrices.Client.ViewModels;
using Xxx.Interview.StockPrices.Service;

namespace Xxx.Interview.StockPrices.Client;

public static class Bootstrapper
{
    private static ILifetimeScope _rootScope;
    private static IMainViewModel _root;

    public static IViewModel RootVisual
    {
        get
        {
            if (_rootScope == null) Start();

            _root = _rootScope.Resolve<IMainViewModel>();
            return _root;
        }
    }

    public static void Start()
    {
        if (_rootScope != null) return;

        var builder = new ContainerBuilder();

        // services & factories

        builder.RegisterType<SchedulerService>()
            .As<ISchedulerService>()
            .SingleInstance();

        builder.RegisterType<ApplicationService>()
            .As<IApplicationService>()
            .SingleInstance();

        builder.RegisterType<MessageService>()
            .As<IMessageService>()
            .SingleInstance();

        builder.RegisterType<GestureService>()
            .As<IGestureService>()
            .SingleInstance();

        builder.RegisterType<StockProviderFactory>()
            .As<IStockProviderFactory>()
            .SingleInstance();

        // view models
        builder.RegisterType<MainViewModel>()
            .As<IMainViewModel>();

        builder.RegisterType<ExceptionViewModel>()
            .As<IExceptionViewModel>();

        _rootScope = builder.Build();
    }

    public static void Stop() => _rootScope.Dispose();

    public static T Resolve<T>()
    {
        if (_rootScope == null) throw new Exception("Bootstrapper hasn't been started!");

        return _rootScope.Resolve<T>(Array.Empty<Parameter>());
    }

    public static T Resolve<T>(Parameter[] parameters)
    {
        if (_rootScope == null) throw new Exception("Bootstrapper hasn't been started!");

        return _rootScope.Resolve<T>(parameters);
    }
}