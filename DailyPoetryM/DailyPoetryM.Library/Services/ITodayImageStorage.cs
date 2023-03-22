using DailyPoetryM.Models;

namespace DailyPoetryM.Services;

public interface ITodayImageStorage
{
    Task<TodayImage> GetTodayImageAsync(bool includingImageStream);

    Task SaveTodayImageAsync(TodayImage todayImage, bool savingExpiresAtOnly);
}