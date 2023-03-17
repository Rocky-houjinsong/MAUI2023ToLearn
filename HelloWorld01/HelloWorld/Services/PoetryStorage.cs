using System.Security.Cryptography.X509Certificates;
using HelloWorld.Models;
using SQLite;

namespace HelloWorld.Services;

public class PoetryStorage : IPoetryStorage
{
    public const string DbFileName = "poetrydb.sqlite3";

    public static readonly string PoetryDbPath =
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder
                .LocalApplicationData), DbFileName);

    private SQLiteAsyncConnection _connection;

    public SQLiteAsyncConnection Connection =>
        _connection ??= new SQLiteAsyncConnection(PoetryDbPath);

    public async Task InitializeAsync() =>
        await Connection.CreateTableAsync<Poetry>();


    public async Task AddAsync(Poetry poetry)
    {
        await Connection.InsertAsync(poetry);
    }

    public async Task<IEnumerable<Poetry>> ListAsync()
    {
        return await Connection.Table<Poetry>().ToListAsync();
    }

    public async Task DeleteAsync()
    {
        await Connection.DeleteAllAsync<Poetry>();
    }
}