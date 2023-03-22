using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryM.Services;

namespace DailyPoetryM.ViewModels;

public class InitializationPageViewModel : ObservableObject
{
    private IPoetryStorage _poetryStorage;

    private IFavoriteStorage _favoriteStorage;

    private IInitializationNavigationService _initializationNavigationService;

    public InitializationPageViewModel(IPoetryStorage poetryStorage,
        IFavoriteStorage favoriteStorage,
        IInitializationNavigationService initializationNavigationService)
    {
        _poetryStorage = poetryStorage;
        _favoriteStorage = favoriteStorage;
        _initializationNavigationService = initializationNavigationService;
        _lazyLoadedCommand =
            new Lazy<AsyncRelayCommand>(
                new AsyncRelayCommand(LoadedCommandFunction));
    }

    public string Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    private string _status = string.Empty;

    public AsyncRelayCommand LoadedCommand => _lazyLoadedCommand.Value;

    private Lazy<AsyncRelayCommand> _lazyLoadedCommand;

    public async Task LoadedCommandFunction()
    {
        if (!_poetryStorage.IsInitialized)
        {
            Status = "正在初始化诗词数据库";
            await _poetryStorage.InitializeAsync();
        }

        if (!_favoriteStorage.IsInitialized)
        {
            Status = "正在初始化收藏数据库";
            await _favoriteStorage.InitializeAsync();
        }

        Status = "所有初始化已完成";
        await Task.Delay(1000);

        _initializationNavigationService.NavigateToAppShell();
    }
}