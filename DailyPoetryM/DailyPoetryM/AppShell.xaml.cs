using DailyPoetryM.Pages;
using DailyPoetryM.Services;

namespace DailyPoetryM;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        var serviceLocatorName = nameof(ServiceLocator);
        var serviceLocator =
            (ServiceLocator)Application.Current.Resources.MergedDictionaries.First(p =>
                p.ContainsKey(serviceLocatorName))[serviceLocatorName];
        var routeService = serviceLocator.RouteService;

        AddFlyoutItem("今日推荐",
            routeService.GetRoute(RootNavigationConstant.TodayPage),
            typeof(TodayPage));
        Routing.RegisterRoute(
            routeService.GetRoute(ContentNavigationConstant.TodayDetailPage),
            typeof(TodayDetailPage));

        AddFlyoutItem("诗词搜索",
            routeService.GetRoute(RootNavigationConstant.QueryPage),
            typeof(QueryPage));
        Routing.RegisterRoute(
            routeService.GetRoute(ContentNavigationConstant.ResultPage),
            typeof(ResultPage));
        Routing.RegisterRoute(
            routeService.GetRoute(ContentNavigationConstant.DetailPage),
            typeof(DetailPage));

        AddFlyoutItem("诗词收藏",
            routeService.GetRoute(RootNavigationConstant.FavoritePage),
            typeof(FavoritePage));
        Routing.RegisterRoute(
            routeService.GetRoute(ContentNavigationConstant.FavoriteDetailPage),
            typeof(DetailPage));

        AddFlyoutItem("关于",
            routeService.GetRoute(RootNavigationConstant.AboutPage),
            typeof(AboutPage));
    }

    private void AddFlyoutItem(string title, string route, Type type) =>
        Items.Add(new FlyoutItem
        {
            Title = title,
            Route = route,
            Items =
            {
                new ShellContent { ContentTemplate = new DataTemplate(type) }
            }
        });
}