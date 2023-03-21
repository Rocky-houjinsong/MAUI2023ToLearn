using System.Linq.Expressions;
using DailyPoetryM.Models;

namespace DailyPoetryM.Services;

public class PoetryStorage : IPoetryStorage
{
    /// <summary>
    /// 判断诗词数据库是否初始化.
    /// </summary>
    /// <remarks>比较偏好存储中和 版本信息中的值;</remarks>
    public bool IsInitialized =>
        Preferences.Get(PoetryStorageConstant.VersionKey, 0) == PoetryStorageConstant.Version;

    public Task InitializedAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Poetry> GetPoetryAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Poetry>> GetPoetriesAsync(Expression<Func<Poetry, bool>> where, int skip, int take)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// 诗词存储常量.
/// </summary>
public static class PoetryStorageConstant
{
    /// <summary>
    /// 正确的版本号.
    /// </summary>
    public const int Version = 1;

    public const string VersionKey = nameof(PoetryStorageConstant) + "." + nameof(Version);
}