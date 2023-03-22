using DailyPoetryM.Pages;

namespace DailyPoetryM.Services;

public class
    InitializationNavigationService : IInitializationNavigationService
{
    private Lazy<InitializationPage> _lazyInitializationPage =
        new(() => new InitializationPage());

    private Lazy<AppShell> _lazyAppShell = new(() => new AppShell());

    public void NavigateToInitializationPage() =>
        Application.Current!.MainPage = _lazyInitializationPage.Value;

    public void NavigateToAppShell() =>
        Application.Current!.MainPage = _lazyAppShell.Value;
}