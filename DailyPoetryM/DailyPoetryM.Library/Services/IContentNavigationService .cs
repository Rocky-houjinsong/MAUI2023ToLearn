namespace DailyPoetryM.Services;

public interface IContentNavigationService
{
    Task NavigateToAsync(string pageKey);

    Task NavigateToAsync(string pageKey, object parameter);
}

public static class ContentNavigationConstant
{
    public const string TodayDetailPage = nameof(TodayDetailPage);

    public const string ResultPage = nameof(ResultPage);

    public const string DetailPage = nameof(DetailPage);

    public const string FavoriteDetailPage = nameof(FavoriteDetailPage);
}