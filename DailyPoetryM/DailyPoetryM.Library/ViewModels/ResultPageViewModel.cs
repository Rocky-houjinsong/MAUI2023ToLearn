using System.Linq.Expressions;
using CommunityToolkit.Mvvm.ComponentModel;
using DailyPoetryM.Models;
using TheSalLab.MauiInfiniteScrolling;

namespace DailyPoetryM.ViewModels;

public class ResultPageViewModel : ObservableObject
{
    private Expression<Func<Poetry, bool>> _where;

    public Expression<Func<Poetry, bool>> Where
    {
        get => _where;
        set => SetProperty(ref _where, value);
    }

    public MauiInfiniteScrollCollection<Poetry> Poetries { get; }
}