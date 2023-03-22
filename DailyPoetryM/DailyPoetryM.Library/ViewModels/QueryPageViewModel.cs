using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryM.Models;
using DailyPoetryM.Services;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace DailyPoetryM.ViewModels;

public class QueryPageViewModel : ObservableObject
{
    private IContentNavigationService contentNavigationService;

    public QueryPageViewModel(
        IContentNavigationService contentNavigationService)
    {
        this.contentNavigationService = contentNavigationService;
        _lazyQueryCommand =
            new Lazy<AsyncRelayCommand>(
                new AsyncRelayCommand(QueryCommandFunction));
        _lazyNavigatedToCommand =
            new Lazy<RelayCommand>(
                new RelayCommand(NavigatedToCommandFunction));
        FilterViewModelCollection.Add(new FilterViewModel(this));
    }

    public ObservableCollection<FilterViewModel> FilterViewModelCollection { get; } = new();

    public virtual PoetryQuery? PoetryQuery
    {
        get => _poetryQuery;
        set => SetProperty(ref _poetryQuery, value);
    }

    private PoetryQuery? _poetryQuery;

    public AsyncRelayCommand QueryCommand => _lazyQueryCommand.Value;

    private Lazy<AsyncRelayCommand> _lazyQueryCommand;

    public async Task QueryCommandFunction()
    {
        // Connection.Table<Poetry>().Where(p => p.Name.Contains("something")
        //                                       && p.AuthorName.Contains("something")
        //                                       && p.Content.Contains("something")
        //                                 ).ToList();

        // p => p.Name.Contains("something")
        //      && p.AuthorName.Contains("something")
        //      && p.Content.Contains("something")

        // p
        var parameter = Expression.Parameter(typeof(Poetry), "p");

        var aggregatedExpression = FilterViewModelCollection
            // Those ViewModels who do have a content.
            .Where(p => !string.IsNullOrWhiteSpace(p.Content))
            // Translate a FilterViewModel to a condition
            // e.g. FilterViewModel {
            //     FileType = {
            //         Name = "作者",
            //         PropertyName = "AuthorName"
            //     },
            //     Content = "苏轼"
            // } => p.AuthorName.Contains("苏轼")
            .Select(p => GetExpression(parameter, p))
            // Put all the conditions together
            // e.g. true && p.AuthorName.Contains("苏轼") && p.Content.Contains("老夫")
            .Aggregate(Expression.Constant(true) as Expression,
                Expression.AndAlso);

        // Turning the expression into a lambda expression
        var where =
            Expression.Lambda<Func<Poetry, bool>>(aggregatedExpression,
                parameter);

        await contentNavigationService.NavigateToAsync(
            ContentNavigationConstant.ResultPage, where);
    }

    public RelayCommand NavigatedToCommand => _lazyNavigatedToCommand.Value;

    private Lazy<RelayCommand> _lazyNavigatedToCommand;

    public void NavigatedToCommandFunction()
    {
        if (_poetryQuery == null) return;

        FilterViewModelCollection.Clear();
        FilterViewModelCollection.Add(new FilterViewModel(this)
        {
            Type = FilterType.NameFilter,
            Content = _poetryQuery.Name
        });
        FilterViewModelCollection.Add(new FilterViewModel(this)
        {
            Type = FilterType.AuthorNameFilter,
            Content = _poetryQuery.Author
        });
        _poetryQuery = null;
    }

    /******** 公开方法 ********/

    /// <summary>
    /// 添加搜索条件ViewModel。
    /// </summary>
    public virtual void AddFilterViewModel(FilterViewModel filterViewModel) =>
        FilterViewModelCollection.Insert(
            FilterViewModelCollection.IndexOf(filterViewModel) + 1,
            new FilterViewModel(this));

    /// <summary>
    /// 删除搜索条件ViewModel。
    /// </summary>
    public virtual void RemoveFilterViewModel(FilterViewModel filterViewModel)
    {
        FilterViewModelCollection.Remove(filterViewModel);
        if (FilterViewModelCollection.Count == 0)
        {
            FilterViewModelCollection.Add(new FilterViewModel(this));
        }
    }

    /******** 私有方法 ********/

    /// <summary>
    /// 获得搜索条件ViewModel的查询语句。
    /// </summary>
    private static Expression GetExpression(ParameterExpression parameter,
        FilterViewModel filterViewModel)
    {
        // parameter => p

        // p.Name or p.AuthorName or p.Content
        var property = Expression.Property(parameter,
            filterViewModel.Type.PropertyName);

        // .Contains()
        var method =
            typeof(string).GetMethod("Contains", new[] { typeof(string) });

        // "something"
        var condition =
            Expression.Constant(filterViewModel.Content, typeof(string));

        // p.Name.Contains("something")
        // or p.AuthorName.Contains("something")
        // or p.Content.Contains("something")
        return Expression.Call(property, method, condition);
    }
}

public class FilterViewModel : ObservableObject
{
    private QueryPageViewModel _queryPageViewModel;

    public FilterViewModel(QueryPageViewModel queryPageViewModel)
    {
        _queryPageViewModel = queryPageViewModel;
        _lazyAddCommand =
            new Lazy<RelayCommand>(new RelayCommand(AddCommandFunction));
        _lazyRemoveCommand =
            new Lazy<RelayCommand>(new RelayCommand(RemoveCommandFunction));
    }

    public FilterType Type
    {
        get => _type;
        set => SetProperty(ref _type, value);
    }

    private FilterType _type = FilterType.NameFilter;

    public string Content
    {
        get => _content;
        set => SetProperty(ref _content, value);
    }

    private string _content;

    public RelayCommand AddCommand => _lazyAddCommand.Value;

    private Lazy<RelayCommand> _lazyAddCommand;

    public void AddCommandFunction() =>
        _queryPageViewModel.AddFilterViewModel(this);

    public RelayCommand RemoveCommand => _lazyRemoveCommand.Value;

    private Lazy<RelayCommand> _lazyRemoveCommand;

    public void RemoveCommandFunction() =>
        _queryPageViewModel.RemoveFilterViewModel(this);
}

public class FilterType
{
    public static readonly FilterType NameFilter =
        new("标题", nameof(Poetry.Name));

    public static readonly FilterType AuthorNameFilter =
        new("作者", nameof(Poetry.Author));

    public static readonly FilterType ContentFilter =
        new("内容", nameof(Poetry.Content));

    public static List<FilterType> FilterTypes { get; } =
        new() { NameFilter, AuthorNameFilter, ContentFilter };

    private FilterType(string name, string propertyName)
    {
        Name = name;
        PropertyName = propertyName;
    }

    public string Name { get; }

    public string PropertyName { get; }
}