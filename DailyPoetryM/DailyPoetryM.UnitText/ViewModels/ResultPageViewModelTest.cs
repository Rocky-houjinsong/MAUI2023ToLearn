using System.Linq.Expressions;
using DailyPoetryM.Models;
using DailyPoetryM.UnitText.Helpers;
using DailyPoetryM.UnitText.Services;
using DailyPoetryM.ViewModels;

namespace DailyPoetryM.UnitText.ViewModels;

public class ResultPageViewModelTest : IDisposable
{
    public ResultPageViewModelTest() => PoetryStorageHelper.RemoveDatabaseFile();
    public void Dispose() => PoetryStorageHelper.RemoveDatabaseFile();

    [Fact]
    public async Task Poetries_Default()
    {
        var where = Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true),
            Expression.Parameter(typeof(Poetry), "p"));
        // //表达式目录树 的翻译为如下语句含义
        // var pList = new List<Poetry> { new Poetry { Id = 1 }, new Poetry { Id = 2 }, new Poetry { Id = 3 } };
        // var poetriesWithGt = pList.Where(p => p.Id == 1);
        var poetryStorage = await PoetryStorageTest.GetInitializedPoetryStorage();
        var resultPageViewModel = new ResultPageViewModel(poetryStorage);
        resultPageViewModel.Where = where;
        //抓取和记录Status属性的所有变化;
        var statusList = new List<string>();
        resultPageViewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(ResultPageViewModel.Status))
            {
                statusList.Add(resultPageViewModel.Status);
            }
        };

        //调用方法
        // resultPageViewModel.NavigatedToCommand.Execute(null); //TODO:RelayCommand的Execute方法多线程问题;
        // await resultPageViewModel.NavigatedToCommandFunction(); //方法一:剥离
        //await resultPageViewModel.NavigatedToCommand01.ExecutionTask; //方法二:AsyncRelayCommand
        await resultPageViewModel.NavigatedToCommandFunction();

        //resultPageViewModel.NavigatedCommand.Execute(null); 
        //await resultPageViewModel.NavigatedCommand.ExecutionTask!; // 方法三:老师的版本,先等待command执行完再执行
        Assert.Equal(ResultPageViewModel.PageSize, resultPageViewModel.Poetries.Count);

        Assert.Equal(2, statusList.Count);
        Assert.Equal(ResultPageViewModel.Loading, statusList[0]);
        Assert.Equal(string.Empty, statusList[1]);
        await poetryStorage.CloseAsync();
    }
}