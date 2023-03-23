using DailyPoetryM.Services;

namespace DailyPoetryM.ViewModels;

[QueryProperty(nameof(PoetryQuery), "parameter")]
public class QueryPageViewModelProxy : QueryPageViewModel
{
    public QueryPageViewModelProxy(
        IContentNavigationService contentNavigationService) : base(
        contentNavigationService)
    {
    }
}