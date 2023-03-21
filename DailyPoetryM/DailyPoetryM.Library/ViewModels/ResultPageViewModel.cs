using System.Linq.Expressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryM.Models;
using DailyPoetryM.Services;
using TheSalLab.MauiInfiniteScrolling;

namespace DailyPoetryM.ViewModels;

public class ResultPageViewModel : ObservableObject
{
    //***********************绑定属性
    private string _status;

    public string Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    private Expression<Func<Poetry, bool>> _where;

    public Expression<Func<Poetry, bool>> Where
    {
        get => _where;
        set => _canLoadMore = SetProperty(ref _where, value);
    }

    public MauiInfiniteScrollCollection<Poetry> Poetries { get; }

    //***********************构造函数
    public ResultPageViewModel(IPoetryStorage poetryStorage)
    {
        Poetries = new MauiInfiniteScrollCollection<Poetry>
        {
            //判断能否加载数据,True还能加载
            OnCanLoadMore = () => _canLoadMore,
            //用来加载数据 回调函数 返回本次新加入的数据;
            OnLoadMore = async () =>
            {
                Status = Loading;
                var poetries = (await poetryStorage.GetPoetriesAsync(Where, Poetries.Count, PageSize)).ToList();
                Status = string.Empty; // 为何要添加这句? 载入数据就一直是Loading,
                if (poetries.Count < PageSize)
                {
                    _canLoadMore = false;
                    Status = NoMoreResult;
                }

                if (Poetries.Count == 0 && poetries.Count == 0)
                {
                    _canLoadMore = false;
                    Status = NoResult;
                }

                return poetries;
            }
        };

        _lazyNavigatedToCommand = new Lazy<AsyncRelayCommand>(new
            AsyncRelayCommand(NavigatedToCommandFunction));
    }

    //*******************************私有变量
    private bool _canLoadMore;
    //老师手动编写的版本,和官方的懒式完全兼容, 修改为 官方版本
    // private RelayCommand _navigatedToCommand;
    //
    // public RelayCommand NavigatedToCommand =>
    //     _navigatedToCommand ??= new RelayCommand(async () => await NavigatedToCommandFunction());

    public async Task NavigatedToCommandFunction()
    {
        Poetries.Clear();
        await Poetries.LoadMoreAsync();
    }

    //懒式初始化方法 -------------->线程不安全,个别情况使用
    // private AsyncRelayCommand _navigatedToCommand01;
    //
    // public AsyncRelayCommand NavigatedToCommand01 =>
    //     _navigatedToCommand01 ??= new AsyncRelayCommand(NavigatedToCommandFunction);

    //官方的懒式初始化写法------------>线程安全
    private Lazy<AsyncRelayCommand> _lazyNavigatedToCommand;

    public AsyncRelayCommand NavigatedToCommand =>
        _lazyNavigatedToCommand.Value;

    //*************************公开常量
    public const int PageSize = 20;
    public const string Loading = "正在载入";
    public const string NoResult = "没有满足条件的结果";
    public const string NoMoreResult = "没有更多结果";
}