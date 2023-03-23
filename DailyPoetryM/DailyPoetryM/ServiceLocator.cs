using DailyPoetryM.Services;
using DailyPoetryM.ViewModels;

namespace DailyPoetryM;

public class ServiceLocator
{
    private IServiceProvider _serviceProvider;

    public ResultPageViewModel ResultPageViewModel =>
        _serviceProvider.GetService<ResultPageViewModel>();


    public TodayPageViewModel TodayPageViewModel =>
        _serviceProvider.GetService<TodayPageViewModel>();

    // public QueryPageViewModelProxy QueryPageViewModelProxy =>
    //     _serviceProvider.GetService<QueryPageViewModelProxy>();


    public InitializationPageViewModel InitializationPageViewModel =>
        _serviceProvider.GetService<InitializationPageViewModel>();

    // public DetailPageViewModelProxy DetailPageViewModelProxy =>
    //     _serviceProvider.GetService<DetailPageViewModelProxy>();

    public FavoritePageViewModel FavoritePageViewModel =>
        _serviceProvider.GetService<FavoritePageViewModel>();

    public AboutPageViewModel AboutPageViewModel =>
        _serviceProvider.GetService<AboutPageViewModel>();

    public IRouteService RouteService =>
        _serviceProvider.GetService<IRouteService>();

    public IPoetryStorage PoetryStorage =>
        _serviceProvider.GetService<IPoetryStorage>();

    public IFavoriteStorage FavoriteStorage =>
        _serviceProvider.GetService<IFavoriteStorage>();

    public IInitializationNavigationService InitializationNavigationService =>
        _serviceProvider.GetService<IInitializationNavigationService>();

    //构造函数 依赖注入容器
    public ServiceLocator()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IPreferenceStorage, PreferenceStorage>(); //注册偏好存储01
        serviceCollection.AddSingleton<IPoetryStorage, PoetryStorage>(); //注册诗词存储02 ,前置01   
        serviceCollection.AddSingleton<IAlertService, AlertService>();
        serviceCollection.AddSingleton<ITodayPoetryService, JinrishiciService>(); //今日诗词服务

        serviceCollection.AddSingleton<ITodayImageStorage, TodayImageStorage>();
        serviceCollection.AddSingleton<ITodayImageService, BingImageService>();

        serviceCollection.AddSingleton<IBrowserService, BrowserService>();

        serviceCollection.AddSingleton<IRouteService, RouteService>();
        serviceCollection
            .AddSingleton<IRootNavigationService, RootNavigationService>();
        serviceCollection
            .AddSingleton<IContentNavigationService,
                ContentNavigationService>();

        serviceCollection.AddSingleton<TodayPageViewModel>(); //今日页
        //serviceCollection.AddSingleton<QueryPageViewModelProxy>();
        serviceCollection.AddSingleton<ResultPageViewModel>(); //注册结果页03 ,前置01,02
        serviceCollection.AddSingleton<InitializationPageViewModel>();
        // serviceCollection.AddSingleton<DetailPageViewModelProxy>();
        serviceCollection.AddSingleton<FavoritePageViewModel>();
        serviceCollection.AddSingleton<AboutPageViewModel>();

        serviceCollection.AddSingleton<IFavoriteStorage, FavoriteStorage>();
        serviceCollection
            .AddSingleton<IInitializationNavigationService,
                InitializationNavigationService>();

        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
}