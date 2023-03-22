namespace DailyPoetryM.Services;

public interface IBrowserService
{
    Task OpenAsync(string url);
}