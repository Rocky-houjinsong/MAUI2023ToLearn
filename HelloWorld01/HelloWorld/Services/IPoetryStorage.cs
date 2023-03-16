using HelloWorld.Models;

namespace HelloWorld.Services;

public interface IPoetryStorage
{
    Task InitializeAsync();
    Task AddAsync(Poetry poetry);

    Task<IEnumerable<Poetry>> ListAsync();
}