using System.Linq.Expressions;
using System.Threading.Channels;
using DailyPoetryM.Models;
using DailyPoetryM.Services;
using DailyPoetryM.UnitText.Helpers;
using Moq;

namespace DailyPoetryM.UnitText.Services;

public class PoetryStorageTest : IDisposable
{
    //初始化需要 清除
    public PoetryStorageTest() => PoetryStorageHelper.RemoveDatabaseFile();

    public void Dispose() => PoetryStorageHelper.RemoveDatabaseFile();

    [Fact]
    public void IsInitialized_Default()
    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        preferenceStorageMock.Setup(p => p.Get(PoetryStorageConstant.VersionKey, 0))
            .Returns(PoetryStorageConstant.Version);
        var mockPreferenceStorage = preferenceStorageMock.Object;
        var poetryStorage = new PoetryStorage(mockPreferenceStorage);
        Assert.True(poetryStorage.IsInitialized);
    }


    [Fact]
    public async Task TestInitializeAsync_Default()
    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        var mockPreferenceStorage = preferenceStorageMock.Object;
        var poetryStorage = new PoetryStorage(mockPreferenceStorage);
        Assert.False(File.Exists(PoetryStorage.PoetryDbPath));
        await poetryStorage.InitializeAsync();
        Assert.True(File.Exists(PoetryStorage.PoetryDbPath));
        preferenceStorageMock.Verify(p => p.Set(PoetryStorageConstant.VersionKey, PoetryStorageConstant.Version),
            Times.Once);
    }

    [Fact]
    public async Task GetPoetryAsync_Default()
    {
        var poetryStorage = await GetInitializedPoetryStorage();
        try
        {
            var poetry = await poetryStorage.GetPoetryAsync(10001);
            Assert.Equal("临江仙 · 夜归临皋", poetry.Name);
        }
        finally
        {
            await poetryStorage.CloseAsync();
        }
    }

    [Fact]
    public async Task GetPoetriesAsync_Default()
    {
        var poetryStorage = await GetInitializedPoetryStorage();
        var poetries = await poetryStorage.GetPoetriesAsync(
            Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true),
                Expression.Parameter(typeof(Poetry), "p")), 0, int.MaxValue);
        // Connection.Table<Poetry>.Where(p => true)
        //select * from Poetry Where TRUE
        Assert.Equal(30, poetries.Count());
        await poetryStorage.CloseAsync();
    }

    public static async Task<PoetryStorage> GetInitializedPoetryStorage()
    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        var mockPreferenceStorage = preferenceStorageMock.Object;
        var poetryStorage = new PoetryStorage(mockPreferenceStorage);
        await poetryStorage.InitializeAsync();
        return poetryStorage;
    }
}