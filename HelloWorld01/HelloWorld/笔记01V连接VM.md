# 前言:

> 本项目 学习的重点在
>
> 1. 最基础的 点击事件完成控件的数据更改
> 2.  :star:  :star: 数据绑定的方式完成 数据的更改;  



* 选择 `MAUI 应用` 模板创建;
* 镜像是为了更快的加载和更新 nuget包;
* 第一次创建 ,后台安装下载需要用到的nuget包;
* 更新为国内的 nuget包源, 点击更新; `https://nuget.cdn.azure.cn/v3/index.json`

新平台 , 编译有些慢;

> 打开 开发者模式 
>
> `设置 --> 隐私和安全性 --> 开发者选项` 

----

<font color = red   >标签里面不许有 空格 ;只要标签和标签属性之间有空格</font>

此时  是 页面 和页面逻辑   虽然分开 ,但是 还是混杂在一起;

前端和后端 最好细分

:interrobang:  怎么做才是对的呢?

:key: 使用MVVM + Service 来制作 ;





课程的MVVM 架构 ,是古典,能够看到 底层的实现逻辑,技术的原理;

读源代码  ;

MAUI, Xamarin, 底层依赖 VUE 3,   UWP   :interrobang: 

**领略工程师的文化**

所有的 开发平台 ,都为 MVVM 提供支持;

MVVM 发明者,提供支持,但没有提供帮助 直接完成;
需要将额外的支持 添加进来; 



> 去年 2021年 ,还都是使用第三方的支持, 使用 `MvvMLight` 来实现
>
> 2022年,终于提供了  官方的实现;  `Community工具包文档` 中的  `MvvM工具包`   社区工具包;
>
> <font  color = red  size = 5> `MVVM工具包` 还是第三方工具包,不过是 微软社区工具包 </font>

**第一方 提供的 第三方工具包 **    英文是   : `CommunityToolkit.MvvM`

微软改名部  ,一个产品,三个名称 

 





# 默认的 展示 

---

控件的展示 和运行

```c#
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloWorld.MainPage">

    <ScrollView>
        <VerticalStackLayout
            Spacing="25"
            Padding="30,0"
            VerticalOptions="Center">

            <Image
                Source="dotnet_bot.png"
                SemanticProperties.Description="Cute dot net bot waving hi to you!"
                HeightRequest="200"
                HorizontalOptions="Center" />

            <Label
                x:Name="ResultLable"
                Text="Hello, World!"
                SemanticProperties.HeadingLevel="Level1"
                FontSize="32"
                HorizontalOptions="Center" />

            <Label
                Text="Welcome to .NET Multi-platform App UI"
                SemanticProperties.HeadingLevel="Level2"
                SemanticProperties.Description="Welcome to dot net Multi platform App U I"
                FontSize="18"
                HorizontalOptions="Center" />

            <Button
                x:Name="CounterBtn"
                Text="Click me"
                SemanticProperties.Hint="Counts the number of times you click"
                Clicked="OnCounterClicked"
                HorizontalOptions="Center" />
            <Button Text="Click Me!"
                     Clicked="Button_OnClicked"></Button>
        </VerticalStackLayout>
    </ScrollView>

</ContentPage>

```

```c#
namespace HelloWorld;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }

    private void Button_OnClicked(object sender, EventArgs e)
    {
        ResultLable.Text = "Hello World! MAUI";
    }
}
```



# 正式工作

---

## 01.导入MvvM工具包

---

在项目的依赖项中 点击选择  `管理 NuGet包`    在浏览选项卡中搜索   CommunityToolkit.MvvM

![image-20230315114341526](https://gitee.com/songhoujin/pictures-to-typora-by-utools/raw/master/image-20230315114341526-2023-3-1511:43:43.png)



三个单词的缩写  Model, View  ViewModel;

> Model : 承载数据
>
> View: 显示数据
>
> ViewModel :
>
> **MVVM 使用的是 数据绑定机制**
>
> <font color = red> View 和 ViewModel绑定起来</font> ,  ViewModel 更新, View 自动更新;  同步进行;
>
> MVC模式 是 Controls 交给Model ,返回给View层;





## 02 架构搭建

---

在项目`HelloWorld`上面 ,不是解决方案上面,进行 添加文件夹

> MVC 有如下几个文件夹 : Models, Controllers, Dao, Pages;
> MVVM 有 如下几个文件夹: Models, Views, ViewModels, Services;

### 第一步:ViewModel

**步骤**

1. add `MainPageViewModel.cs`  
   ![image-20230315131859417](https://gitee.com/songhoujin/pictures-to-typora-by-utools/raw/master/image-20230315131859417-2023-3-1513:19:00.png)



新语法,支持 以命名空间 来规定 一个文件;

2. 基础设定 ,继承,修改访问权限等操作
   * 简写 obob  快速继承;
   * 修改为public
   * 蓝色 是关键字;
3. 编写属性 `Result`  是 ViewModel 为 View提供 的 数据;   --> 搞定数据  , 可读可写
   属性 Result的值,其实 是 字段变量_result的值;
4. 编写 点击事件   `RelayCommand`  点击事件 , 该命令也是一个属性; 只读command, 

>  数据承载 
>
> 点击事件

```c#
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HelloWorld.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        private string _result = string.Empty;

        public string Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        private RelayCommand _clickMeCommand;

        public RelayCommand ClickMeCommand =>
            _clickMeCommand ??= new RelayCommand(() => Result = "Hello World! MAUI");
    }
}
```



**原理阐述**



此时的哈 ,ViewModel 给 View提供数据 ; 

### :interrobang:  **View 如何找到 ViewModel呢?**



设计模式:  <font color = red> 服务定位器模式  </font>   依赖注入

**步骤**

* 项目上添加类`ServiceLocator` 
  * 去除无用引用
  * 命名空间变为分号形式
    ![image-20230315134824450](https://gitee.com/songhoujin/pictures-to-typora-by-utools/raw/master/image-20230315134824450-2023-3-1513:48:25.png)
  * public

没有原生和集成, 

**服务定位器 ,提供定位 帮助View找ViewModel**



### 第二步: ServiceLocator



* 定义属性  ,提供 服务 

> private IServiceProvider _serviceProvider;
>
> 微软提供  `Defines a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.`
> **服务提供者:**  广义的服务, 理解成 ,提供各种各样的对象;

* 定义属性 
  * MainPageViewModel  类型
    * 调用该属性 , 调用 服务的 getservice函数,返回 该类型的实例
    * Java的泛型的伪泛型; C#的真 泛型 ;
    * 依赖注入的原理, IoC

> :star:   属性 是 从 `口袋` 里面拿出想要的 实例对象, 但 前提是 需要有操作来完成 存放实例对象 ,才能拿出来
>
> :key:  **在ServiceLocator的构造函数中存放**
>
> 单件模式(单例模式的运用)  `AddSingleton`

* ServiceCollection  和  IServiceProvider  是 两个东西 

  * 前者是 服务对象集合 , 后者的提供服务对象, 需要二者建立 连接

    ```c#
    _serviceProvider = serviceCollection.BuildServiceProvider()
    ```



<font color = gree size = 5 > 总结</font>

服务定位器 这个类 ;  

构造器收集需要提供的 对象; 

借助属性 来提供指定你需要的对象

### 第三步:注册serviceLocator

---

> View  , ViewModel  , ServiceLocator;
>
> * 借助 ServiceLocator  可以找到 ViewModel 
>   :interrobang:  那我如何找到 ServiceLocator呢?  
>
>   :key:  **通过资源resource**   ==在 `App.xaml` 里面   ,将`ServiceLocator` 定义成资源==



> 应用 , 应用资源, 资源字典,   合并字典

```xaml
<?xml version = "1.0" encoding = "UTF-8" ?>
<Application xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:HelloWorld"
             x:Class="HelloWorld.App">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="Resources/Styles/Colors.xaml" />
                <ResourceDictionary Source="Resources/Styles/Styles.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>

```

将该资源 起名字, ,虽然名字一样,但是 前者是资源类型,后者是 资源名称

```xaml
 <!--将ServiceLocator注册成资源-->
                <ResourceDictionary>
                    <local:ServiceLocator x:Key="ServiceLocator"></local:ServiceLocator>
                </ResourceDictionary>
```

在应用程序范围内,只要 使用该名字,就能访问到 该资源

### 第四步: 联系View 和ViewModel

----

标准写法; 是古典写法, 是 开发性能的写法; 大幅度提升开发效率的写法;

>  在`MainPage.xaml` 的名称空间中  , 使用函数  ,bindingcontext,  导入 并绑定 

```c#
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             BindingContext="{Binding MainPageViewModel,Source={StaticResource ServiceLocator}}"
             x:Class="HelloWorld.MainPage">
```



> **前面的label 通过button点击事件修改不删除** 
>
> 和本次 通过 数据绑定的方式 



Button 有 `command` 属性 含义为 button 所能执行的命令;

```c#
  <!--数据绑定,更改控件数据-->
            <Label Text="{Binding Result}"></Label>
            <Button Text="数据绑定"
                    Command="{Binding ClickMeCommand}"></Button>
```



### :star: 总结

> 对整套流程进行总结 回顾

1. Button 点击 ,通过 command这个属性绑定的 `ClickCommand` 事件 来 触发;

   但是  通过绑定的方式 ,需要找到该 事件, 

   名称空间 中   说 ,是 绑定的是  资源中的 MainPageViewModel

   返回到 app 的资源字典中查找; 

   找到资源定位器 ,     资源定位器的 属性返回 viewmodel实例

   然后viewmodel 中的属性 result 改变了,  view中的lable绑定的属性也就随之改变;

   

![image-20230315150303205](https://gitee.com/songhoujin/pictures-to-typora-by-utools/raw/master/image-20230315150303205-2023-3-1515:03:04.png)



View  单方面的 关注 ViewModel, 而 ViewModel 不关注 也不依赖 View;

单方面的 依赖  好控制

有向 无环图   Vue 不是完全遵循 mvvm



古典的, 看到整套 mvvm  是如何形成 的;

* view  是如何被 viewModel找到的 ? `ServiceLocator`
* service是 如何 解决 service 和 viewmodel 的关联关系的?  `依赖注入 IoC`
* service是如何注册成资源的?  `App.xaml 注册资源字典` 
* view是 怎么绑定viewmodel的?  `绑定上下文` 





------------------

使用模板 MAUI Blazor ,Web方式创建多平台应用 

MvvM细节全部隐藏 

![image-20230315152021287](https://gitee.com/songhoujin/pictures-to-typora-by-utools/raw/master/image-20230315152021287-2023-3-1515:20:22.png)

微信小程序 也是如此 



MvvM 是现代开发 普遍存在的事实了;

<font color = red size = 4> 下学期 讲 微服务与架构模式!!!!</font>

下一讲:

MVVM架构下的 几种  数据操作  ,

介绍一点前端

键值数据库 ,  嵌入式数据库 Sqlite,,介绍代码优先方法  code-first来操作数据库;

不需要数据库 , 让类自动变成数据库表;



# B站评论总结:



> 用的是servicelocator模式，这种模式存在一个限制，==所有的viewmodel只能是单例的==，如果同一个视图存在了多个示例，那么这种方法就存在问题了，这种情况下，应该指定datatype才能获得编译时提示



> windows和android它们的界面布局怎么自适应呀，总是调好一个，另一个就乱了

* Onplatform
