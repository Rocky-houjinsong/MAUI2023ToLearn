namespace DailyPoetryM.Models;

/// <summary>
/// 今日诗词API返回.
/// </summary>
public class TodayPoetry
{
    public string Snippet { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    //朝代
    public string Dynasty { get; set; } = string.Empty;

    //作者
    public string Author { get; set; } = string.Empty;

    //内容
    public string Content { get; set; } = string.Empty;

    //数据来源_处理特殊情况
    public string Source { get; set; } = string.Empty;
}