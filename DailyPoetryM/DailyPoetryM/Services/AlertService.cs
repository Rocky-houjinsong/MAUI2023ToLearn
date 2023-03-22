namespace DailyPoetryM.Services;

public class AlertService : IAlertService
{
    public void Alert(string title, string message, string button) =>
        MainThread.BeginInvokeOnMainThread(async () =>
            await Shell.Current.DisplayAlert(title, message, button));
}