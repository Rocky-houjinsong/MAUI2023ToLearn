using DailyPoetryM.Pages;

namespace DailyPoetryM;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        //在导航栏中新添加导航项
        Items.Add(new FlyoutItem
        {
            Title = nameof(ResultPage), //标题
            Route = nameof(ResultPage), //路径
            Items = //内容
            {
                new ShellContent
                {
                    ContentTemplate = new DataTemplate(typeof(ResultPage))
                }
            }
        });
    }
}