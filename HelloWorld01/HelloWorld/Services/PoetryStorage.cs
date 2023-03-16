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

    public async Task Initialize() =>
        await Connection.CreateTableAsync<Poetry>();


    public void Add(Poetry poetry)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Poetry> List()
    {
        throw new NotImplementedException();
    }
}