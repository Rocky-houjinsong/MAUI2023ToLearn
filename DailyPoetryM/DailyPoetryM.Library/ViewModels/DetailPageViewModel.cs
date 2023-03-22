using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryM.Models;
using DailyPoetryM.Services;

namespace DailyPoetryM.ViewModels;

public class DetailPageViewModel : ObservableObject
{
    private IFavoriteStorage _favoriteStorage;

    public DetailPageViewModel(IFavoriteStorage favoriteStorage)
    {
        _favoriteStorage = favoriteStorage;
        _lazyNavigatedToCommand =
            new Lazy<AsyncRelayCommand>(
                new AsyncRelayCommand(NavigatedToCommandFunction));
        _lazyFavoriteToggledCommand =
            new Lazy<AsyncRelayCommand>(
                new AsyncRelayCommand(FavoriteToggledCommandFunction));
    }

    public Poetry? Poetry
    {
        get => _poetry;
        set => SetProperty(ref _poetry, value);
    }

    private Poetry? _poetry;

    public Favorite? Favorite
    {
        get => _favorite;
        set => SetProperty(ref _favorite, value);
    }

    private Favorite? _favorite;

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    private bool _isLoading;

    public bool IsFavorite { get; set; }

    public AsyncRelayCommand NavigatedToCommand =>
        _lazyNavigatedToCommand.Value;

    private Lazy<AsyncRelayCommand> _lazyNavigatedToCommand;

    public async Task NavigatedToCommandFunction()
    {
        IsLoading = true;
        var favorite = await _favoriteStorage.GetFavoriteAsync(Poetry.Id) ??
                       new Favorite { PoetryId = Poetry.Id };
        IsFavorite = favorite.IsFavorite;
        Favorite = favorite;
        IsLoading = false;
    }

    public AsyncRelayCommand FavoriteToggledCommand =>
        _lazyFavoriteToggledCommand.Value;

    private Lazy<AsyncRelayCommand> _lazyFavoriteToggledCommand;

    public async Task FavoriteToggledCommandFunction()
    {
        if (IsFavorite == Favorite.IsFavorite)
        {
            return;
        }

        IsFavorite = Favorite.IsFavorite;

        IsLoading = true;
        await _favoriteStorage.SaveFavoriteAsync(Favorite);
        IsLoading = false;
    }
}