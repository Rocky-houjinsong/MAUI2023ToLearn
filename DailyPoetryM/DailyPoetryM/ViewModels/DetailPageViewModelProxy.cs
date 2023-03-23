using DailyPoetryM.Services;

namespace DailyPoetryM.ViewModels;

[QueryProperty(nameof(Poetry), "parameter")]
public class DetailPageViewModelProxy : DetailPageViewModel
{
    public DetailPageViewModelProxy(IFavoriteStorage favoriteStorage) : base(
        favoriteStorage)
    {
    }
}