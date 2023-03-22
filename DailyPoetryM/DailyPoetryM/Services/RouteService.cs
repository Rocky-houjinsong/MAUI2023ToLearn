namespace DailyPoetryM.Services;

public class RouteService : IRouteService
{
    private readonly Dictionary<string, string> _routeDictionary = new()
    {
        [RootNavigationConstant.TodayPage] = RootNavigationConstant.TodayPage,
        [ContentNavigationConstant.TodayDetailPage] =
            $"{RootNavigationConstant.TodayPage}/{ContentNavigationConstant.TodayDetailPage}",
        [RootNavigationConstant.QueryPage] = RootNavigationConstant.QueryPage,
        [ContentNavigationConstant.ResultPage] =
            $"{RootNavigationConstant.QueryPage}/{ContentNavigationConstant.ResultPage}",
        [ContentNavigationConstant.DetailPage] =
            $"{RootNavigationConstant.QueryPage}/{ContentNavigationConstant.ResultPage}/{ContentNavigationConstant.DetailPage}",
        [RootNavigationConstant.FavoritePage] =
            RootNavigationConstant.FavoritePage,
        [ContentNavigationConstant.FavoriteDetailPage] =
            $"{RootNavigationConstant.FavoritePage}/{ContentNavigationConstant.FavoriteDetailPage}",
        [RootNavigationConstant.AboutPage] = RootNavigationConstant.AboutPage
    };

    public string GetRoute(string pageKey) => _routeDictionary[pageKey];
}