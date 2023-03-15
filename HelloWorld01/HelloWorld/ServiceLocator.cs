using HelloWorld.ViewModels;

namespace HelloWorld;

public class ServiceLocator
{
    private IServiceProvider _serviceProvider;

    public MainPageViewModel MainPageViewModel =>
        _serviceProvider.GetService<MainPageViewModel>(); // 依赖注入; IoC

    public ServiceLocator()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<MainPageViewModel>();
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
}