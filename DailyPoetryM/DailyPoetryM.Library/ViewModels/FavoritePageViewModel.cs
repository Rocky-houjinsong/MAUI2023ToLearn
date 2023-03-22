using CommunityToolkit.Mvvm.Input;
using DailyPoetryM.Models;
using DailyPoetryM.Services;
using ObservableObject = CommunityToolkit.Mvvm.ComponentModel.ObservableObject;

namespace DailyPoetryM.ViewModels;

public class FavoritePageViewModel : ObservableObject
{
    private IFavoriteStorage _favoriteStorage;

    private IPoetryStorage _poetryStorage;

    IContentNavigationService _contentNavigationService;

    public FavoritePageViewModel(IFavoriteStorage favoriteStorage,
        IPoetryStorage poetryStorage,
        IContentNavigationService contentNavigationService)
    {
        _favoriteStorage = favoriteStorage;
        _poetryStorage = poetryStorage;
        _contentNavigationService = contentNavigationService;

        _favoriteStorage.Updated += FavoriteStorageOnUpdated;

        _lazyLoadedCommand =
            new Lazy<AsyncRelayCommand>(
                new AsyncRelayCommand(LoadedCommandFunction));
        _layzPoetryTappedCommand = new Lazy<AsyncRelayCommand<PoetryFavorite>>(
            new AsyncRelayCommand<PoetryFavorite>(PoetryTappedCommandFunction));
    }

    public ObservableRangeCollection<PoetryFavorite> PoetryFavoriteCollection { get; } = new();

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    private bool _isLoading;

    public AsyncRelayCommand LoadedCommand => _lazyLoadedCommand.Value;

    private Lazy<AsyncRelayCommand> _lazyLoadedCommand;

    public async Task LoadedCommandFunction()
    {
        IsLoading = true;

        PoetryFavoriteCollection.Clear();
        var favoriteList = await _favoriteStorage.GetFavoritesAsync();

        PoetryFavoriteCollection.AddRange((await Task.WhenAll(
            favoriteList.Select(p => Task.Run(async () => new PoetryFavorite
            {
                Poetry = await _poetryStorage.GetPoetryAsync(p.PoetryId),
                Favorite = p
            })))).ToList());

        IsLoading = false;
    }

    public AsyncRelayCommand<PoetryFavorite> PoetryTappedCommand =>
        _layzPoetryTappedCommand.Value;

    private Lazy<AsyncRelayCommand<PoetryFavorite>> _layzPoetryTappedCommand;

    public async Task
        PoetryTappedCommandFunction(PoetryFavorite poetryFavorite) =>
        await _contentNavigationService.NavigateToAsync(
            ContentNavigationConstant.FavoriteDetailPage,
            poetryFavorite.Poetry);


    private async void FavoriteStorageOnUpdated(object? sender,
        FavoriteStorageUpdatedEventArgs e)
    {
        var favorite = e.UpdatedFavorite;
        PoetryFavoriteCollection.Remove(
            PoetryFavoriteCollection.FirstOrDefault(p =>
                p.Favorite.PoetryId == favorite.PoetryId));

        if (!favorite.IsFavorite)
        {
            return;
        }

        var poetryFavorite = new PoetryFavorite
        {
            Poetry = await _poetryStorage.GetPoetryAsync(favorite.PoetryId),
            Favorite = favorite
        };

        var index = PoetryFavoriteCollection.IndexOf(
            PoetryFavoriteCollection.FirstOrDefault(p =>
                p.Favorite.Timestamp < favorite.Timestamp));
        if (index < 0)
        {
            index = PoetryFavoriteCollection.Count;
        }

        PoetryFavoriteCollection.Insert(index, poetryFavorite);
    }
}

public class PoetryFavorite
{
    public Poetry Poetry { get; set; }

    public Favorite Favorite { get; set; }
}