using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryM.Models;
using DailyPoetryM.Services;

namespace DailyPoetryM.ViewModels;

public class TodayPageViewModel : ObservableObject
{
    #region 构造函数

    private ITodayImageService _todayImageService;

    //读取 今日诗词
    private ITodayPoetryService _todayPoetryService;

    private IContentNavigationService _contentNavigationService;

    private IRootNavigationService _rootNavigationService;

    private IBrowserService _browserService;

    private TodayImage? _todayImage;

    private TodayPoetry? _todayPoetry;

    private bool _isLoading;

    //依赖注入 获得如下
    public TodayPageViewModel(ITodayImageService todayImageService,
        ITodayPoetryService todayPoetryService,
        IContentNavigationService contentNavigationService,
        IRootNavigationService rootNavigationService,
        IBrowserService browserService)
    {
        _todayImageService = todayImageService;
        _todayPoetryService = todayPoetryService;
        _contentNavigationService = contentNavigationService;
        _rootNavigationService = rootNavigationService;
        _browserService = browserService;
        _lazyLoadedCommand =
            new Lazy<RelayCommand>(new RelayCommand(LoadedCommandFunction)); //页面加载
        _lazyShowDetailCommand =
            new Lazy<AsyncRelayCommand>(
                new AsyncRelayCommand(ShowDetailCommandFunction));
        _lazyJinrishiciCommand =
            new Lazy<AsyncRelayCommand>(
                new AsyncRelayCommand(JinrishiciCommandFunction));
        _lazyCopyrightCommand =
            new Lazy<AsyncRelayCommand>(
                new AsyncRelayCommand(CopyrightCommandFunction));
        _lazyQueryCommand =
            new Lazy<AsyncRelayCommand>(
                new AsyncRelayCommand(QueryCommandFunction));
    }

    #endregion

    public TodayImage? TodayImage
    {
        get => _todayImage;
        set => SetProperty(ref _todayImage, value);
    }

    public TodayPoetry? TodayPoetry
    {
        get => _todayPoetry;
        set => SetProperty(ref _todayPoetry, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public RelayCommand LoadedCommand => _lazyLoadedCommand.Value;

    private readonly Lazy<RelayCommand> _lazyLoadedCommand;

    public void LoadedCommandFunction()
    {
        Task.Run(async () =>
        {
            TodayImage = await _todayImageService.GetTodayImageAsync();

            var updateResult = await _todayImageService.CheckUpdateAsync();
            if (updateResult.HasUpdate)
            {
                TodayImage = updateResult.TodayImage;
            }
        });

        Task.Run(async () =>
        {
            IsLoading = true;
            TodayPoetry = await _todayPoetryService.GetTodayPoetryAsync();
            IsLoading = false;
        });
    }

    public AsyncRelayCommand ShowDetailCommand => _lazyShowDetailCommand.Value;

    private readonly Lazy<AsyncRelayCommand> _lazyShowDetailCommand;

    public async Task ShowDetailCommandFunction() =>
        await _contentNavigationService.NavigateToAsync(
            ContentNavigationConstant.TodayDetailPage);


    public AsyncRelayCommand JinrishiciCommand => _lazyJinrishiciCommand.Value;

    private readonly Lazy<AsyncRelayCommand> _lazyJinrishiciCommand;

    public async Task JinrishiciCommandFunction() =>
        await _browserService.OpenAsync("https://www.jinrishici.com");

    public AsyncRelayCommand CopyrightCommand => _lazyCopyrightCommand.Value;

    private readonly Lazy<AsyncRelayCommand> _lazyCopyrightCommand;

    public async Task CopyrightCommandFunction() =>
        await _browserService.OpenAsync(TodayImage.CopyrightLink);

    public AsyncRelayCommand QueryCommand => _lazyQueryCommand.Value;

    private readonly Lazy<AsyncRelayCommand> _lazyQueryCommand;

    public async Task QueryCommandFunction() =>
        await _rootNavigationService.NavigateToAsync(
            RootNavigationConstant.QueryPage,
            new PoetryQuery
            {
                Author = TodayPoetry.Author,
                Name = TodayPoetry.Name
            });
}