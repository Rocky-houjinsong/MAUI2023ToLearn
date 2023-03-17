using HelloWorld.Services;
using HelloWorld.ViewModels;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
        // serviceCollection.AddSingleton<IKeyValueStorage, KeyValueStorage>();
        serviceCollection.AddSingleton<IPoetryStorage, PoetryStorage>();
        serviceCollection.AddSingleton<ITokenService, TokenService>();
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
}