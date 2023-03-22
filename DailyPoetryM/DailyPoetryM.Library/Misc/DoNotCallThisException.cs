namespace DailyPoetryM.Misc;

public class DoNotCallThisException : Exception
{
    public DoNotCallThisException() : base("不应该调用此项目。")
    {
    }
}