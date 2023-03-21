namespace DailyPoetryM.Services;

public class PreferenceStorage : IPreferenceStorage
{
    public void Set(string key, int value) => Set(key, value);

    public int Get(string key, int defaultValue) => Get(key, defaultValue);
}