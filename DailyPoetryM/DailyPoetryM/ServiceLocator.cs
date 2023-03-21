using DailyPoetryM.Services;
using DailyPoetryM.ViewModels;

namespace DailyPoetryM;

public class ServiceLocator
{
    private IServiceProvider _serviceProvider;

    public ResultPageViewModel ResultPageViewModel =>
        _serviceProvider.GetService<ResultPageViewModel>();

    //构造函数 依赖注入容器
    public ServiceLocator()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IPreferenceStorage, PreferenceStorage>(); //注册偏好存储01
        serviceCollection.AddSingleton<IPoetryStorage, PoetryStorage>(); //注册诗词存储02 ,前置01   
        serviceCollection.AddSingleton<ResultPageViewModel>(); //注册结果页03 ,前置01,02
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
}