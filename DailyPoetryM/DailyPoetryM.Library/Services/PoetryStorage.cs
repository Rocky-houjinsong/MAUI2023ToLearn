using System.Linq.Expressions;
using DailyPoetryM.Models;
using SQLite;

namespace DailyPoetryM.Services;

public class PoetryStorage : IPoetryStorage
{
    //*******************************私有变量
    private readonly IPreferenceStorage _preferenceStorage;

    //***********************构造函数
    public PoetryStorage(IPreferenceStorage preferenceStorage)
    {
        _preferenceStorage = preferenceStorage;
    }

    //*************************公开变量
    public const string DbName = "poetrydb.sqlite3";

    public static readonly string PoetryDbPath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbName);

    private SQLiteAsyncConnection _connection;

    public SQLiteAsyncConnection Connection =>
        _connection ??= new SQLiteAsyncConnection(PoetryDbPath);

    /// <summary>
    /// 判断诗词数据库是否初始化.
    /// </summary>
    /// <remarks>比较偏好存储中和 版本信息中的值;</remarks>
    public bool IsInitialized =>
        _preferenceStorage.Get(PoetryStorageConstant.VersionKey, 0) == PoetryStorageConstant.Version;

    public async Task InitializedAsync()
    {
        await using var dbFileStream =
            new FileStream(PoetryDbPath, FileMode.OpenOrCreate);

        await using var dbAssetStream =
            typeof(PoetryStorage)
                .Assembly
                .GetManifestResourceStream(DbName) ?? throw new Exception($"找不到名为{DbName}的资源");
        await dbAssetStream.CopyToAsync(dbFileStream);
        _preferenceStorage.Set(PoetryStorageConstant.VersionKey, PoetryStorageConstant.Version);
    }

    public async Task<Poetry> GetPoetryAsync(int id) =>
        await Connection.Table<Poetry>().FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Poetry>> GetPoetriesAsync(Expression<Func<Poetry, bool>> where, int skip, int take) =>
        await Connection.Table<Poetry>().Where(where).Skip(skip).Take(take).ToListAsync();

    public async Task CloseAsync() => await Connection.CloseAsync();
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