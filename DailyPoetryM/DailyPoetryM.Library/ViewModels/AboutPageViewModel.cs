using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryM.Services;

namespace DailyPoetryM.ViewModels;

public class AboutPageViewModel : ObservableObject
{
    private IBrowserService _browserService;

    public AboutPageViewModel(IBrowserService browserService)
    {
        _browserService = browserService;
        _lazyOpenUrlCommand =
            new Lazy<AsyncRelayCommand<string>>(
                new AsyncRelayCommand<string>(OpenUrlCommandFunction));
    }

    public AsyncRelayCommand<string> OpenUrlCommand =>
        _lazyOpenUrlCommand.Value;

    private Lazy<AsyncRelayCommand<string>> _lazyOpenUrlCommand;

    public async Task OpenUrlCommandFunction(string url) =>
        await _browserService.OpenAsync(url);
}