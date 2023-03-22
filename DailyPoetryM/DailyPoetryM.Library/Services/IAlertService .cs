namespace DailyPoetryM.Services;

/// <summary>
/// 提示信息.
/// </summary>
public interface IAlertService
{
    void Alert(string title, string message, string button);
}