namespace DailyPoetryM;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        //MainPage = new AppShell();
        MainPage = new NavigationPage(new ContentPage());

        var serviceLocatorName = nameof(ServiceLocator);
        var serviceLocator =
            (ServiceLocator)Application.Current.Resources.MergedDictionaries
                .First(p => p.ContainsKey(serviceLocatorName))[
                    serviceLocatorName];

        var poetryStorage = serviceLocator.PoetryStorage;
        var favoriteStorage = serviceLocator.FavoriteStorage;
        var initializationNavigationService = serviceLocator.InitializationNavigationService;

        if (!poetryStorage.IsInitialized || !favoriteStorage.IsInitialized)
        {
            initializationNavigationService.NavigateToInitializationPage();
        }
        else
        {
            initializationNavigationService.NavigateToAppShell();
        }
    }
}