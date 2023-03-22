using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace DailyPoetryM.Models;

public class Favorite : ObservableObject
{
    [PrimaryKey] public int PoetryId { get; set; }

    private bool _isFavorite;

    public virtual bool IsFavorite
    {
        get => _isFavorite;
        set => SetProperty(ref _isFavorite, value);
    }

    public long Timestamp { get; set; }
}