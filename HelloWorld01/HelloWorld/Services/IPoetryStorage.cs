using HelloWorld.Models;

namespace HelloWorld.Services;

public interface IPoetryStorage
{
    Task Initialize();
    void Add(Poetry poetry);

    IEnumerable<Poetry> List();
}