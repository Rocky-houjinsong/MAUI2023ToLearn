# 01 数据库:读取数据库内容

---

 嵌入式数据库 SQLite

访问数据库, 建表,对应类 

first-code;

第一次 介绍 Model ; 

> Model 只承载数据; 
>
> 小型项目  安装 `sqlite-net-pcl` 包

**步骤**

## 01:Model层 Poetry类

---

导入包, 添加标记;

标记含义  主键,自增;



## 02:service层

个人习惯: 把 所有 和数据存储的  命名为 XXXStorage

先编写接口 ,规定 方法: 初始化, 添加, 列表化



**实现接口**

嵌入式数据局库 ,   是嵌入到软件程序里面的 ;

是以文件的形式进行保存;



*   各个平台的文件系统 不一致;
  多平台开发 需要 落实到各平台的访问上面;

  > DotNET 平台上 提供了支持 

* 蓝色下划线, 可空类型 ;
  

----

## 03实现service:PoetryStorage

---

* 文件名  和文件路径 ,使用const 和 static readonly 
* 编译 理论是 无法编辑的, 但C# 现在 是  可以支持编译了

> **开发小技巧**

* 函数名称修改  refactor --> Rename   `F2`
* 方法签名修改  refactor --> Change Signature;; `Ctrl + F6`

  



---

以上完成的是 `IPoetryStorage`接口以及其实现类`PoetryStorage`;

* 接口先声明方法
* 实现方法
* 返回修改/重构接口;

```c#
using HelloWorld.Models;

namespace HelloWorld.Services;

public interface IPoetryStorage
{
    Task InitializeAsync();
    Task AddAsync(Poetry poetry);

    Task<IEnumerable<Poetry>> ListAsync();
}
```



```c#
using System.Security.Cryptography.X509Certificates;
using HelloWorld.Models;
using SQLite;

namespace HelloWorld.Services;

public class PoetryStorage : IPoetryStorage
{
    public const string DbFileName = "poetrydb.sqlite3";

    public static readonly string PoetryDbPath =
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder
                .LocalApplicationData), DbFileName);

    private SQLiteAsyncConnection _connection;

    public SQLiteAsyncConnection Connection =>
        _connection ??= new SQLiteAsyncConnection(PoetryDbPath);

    public async Task InitializeAsync() =>
        await Connection.CreateTableAsync<Poetry>();


    public async Task AddAsync(Poetry poetry)
    {
        await Connection.InsertAsync(poetry);
    }

    public async Task<IEnumerable<Poetry>> ListAsync()
    {
        return await Connection.Table<Poetry>().ToListAsync();
    }
}
```

## 04注册服务

> 以上编写 访问数据库进行 对诗词的 初始化, 插入和读取, ;
>
> 这个需要提供给 ViewModel, 那么就涉及到 第一章节 学习的 
> :star:  **通过ServiceLocator依赖注入将Service提供给ViewModel**

```
serviceCollection.AddSingleton<IPoetryStorage, PoetryStorage>();
```



## 05重写MainPageViewModel

---



:key: **构造函数自动化生成 : Alt , Fn+Insert**
`ctor` 也是可以生成的

`Alt + Enter`  选择 `生成代码`  选择也是可以的;

----

 `ObservableCollection` 理解为 数组就可以;

---

:interrobang:  ==属性的区分==

> 以下二者是  含义一样的不同写法

```c#
  public ObservableCollection<Poetry> Poetries2
        {
            get => new ObservableCollection<Poetry>();
        }

        public ObservableCollection<Poetry> Poetries3 =>
            new ObservableCollection<Poetry>();
//点击变量, 点击灯泡, 选择变为表达式体
```

![image-20230316095922720](https://gitee.com/songhoujin/pictures-to-typora-by-utools/raw/master/image-20230316095922720-2023-3-1609:59:24.png)



> Kotlin语言中的 也会出现 这样的细节

> 前者 : 生成 一次, 调用无数次
> 后者: 生成一次,调用一次;

* 前者:属性初始化器, 相当于 隐式构造函数
* 后者:只读属性的lambda语法,隐式定义 get访问器的方法体

```c#
public ObservableCollection<Poetry> Poetries { get; } =
            new ObservableCollection<Poetry>();
             public ObservableCollection<Poetry> Poetries3 =>
            new ObservableCollection<Poetry>();
```



**之后编写3个命令:RelayCommand 涉及到 命令的初始化, 插入,读取**





取数据的方法,微软官方都吐槽 ;





对控件的学习 

*  通过 MAUI的官方文档;
* Xamarin 视频;



<font color = red> Libre Office Community</font> 用来画 UI设计图 ;

> 步骤:编写MainPageViewModel;
>
> `*` 有多少空间 占多少空间
>
> `auto` 自动变化

UI 这个东西 , 水深, 可移植性不是太强;

不用太多的时间花在UI上

关注性能, 关注 DeBug,关注 优化

---

 按钮的功能进行 关联起来;

> 使用 ListView 来显示数据;  最新版本使用的是 `CollectionView`

`ItemTemplate`  项目模板; 
`DataTemplate` 数据模板;
`ViewCell` 
`VerticalStackLayout` 栈式布局排版



Button上 的Comman 绑定的是 ViewModel

ListView 上面绑定的是 Poetry,也就是Model的属性

绑定上下文切换, 有 设计思想的体现;

MAUI  读法 ,马鸥一 ;

.NET  ,读法 逗net

使用WSA 进行android端测试;



对`Poetry` 多个属性 综合生成  lable ,该怎么显示呢?

> 将 title 和 content  拼接;

* 虚拟化属性  --> 改变业务逻辑  [Ingore]标记
* 数据转换器 --> 只适用UI层
* wrapper类 ,将poetry类包装起来; 适配器模式;  -->万能,但是 代码有点多



## 06View控件绑定属性

---

```xaml
 <CollectionView Grid.Row="0" 
                         Grid.Column="0" 
                         Grid.ColumnSpan="3" 
                         ItemsSource="{Binding Poetries}">
                <CollectionView.ItemTemplate>
                <DataTemplate>

                    <VerticalStackLayout Padding="8">
                        <Label Text="{Binding Title}" FontSize="22"></Label>
                        <Label Text="{Binding Content}"></Label>
                    </VerticalStackLayout>

                </DataTemplate>
        </CollectionView.ItemTemplate>
                 
             
         </CollectionView> 
```





# Web服务:Json

:key: **使用模板快速生成代码**



>  **编写`访问Web服务`**

访问Web 服务,就是一个异步服务调用过程;

示例中的访问web 很简单,  

之后有严格的访问方法;

```c#
public interface ITokenService
{
    Task<string> GetTokenAsync();
}
```

```c#
 public class TokenService : ITokenService
    {
        public async Task<string> GetTokenAsync()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://v2.jinrishici.com/token");
            var json = await response.Content.ReadAsStringAsync();
            return json;
        }
    }
```



> **注册服务**   servicelocator

```c#
serviceCollection.AddSingleton<ITokenService, TokenService>();
```



> **读取数据**  
>
> ViewModel 中 以 属性的方式  承载数据

```c#
  public string Json
        {
            get => _json;
            set => SetProperty(ref _json, value);
        }

        /// <summary>
        /// Json属性.
        /// </summary>
        private string _json;
```



> **使用服务**
>
> 以可绑定命令的方式调用 服务

```c#
  private string _json;

        private RelayCommand _loadJson;

        public RelayCommand LoadJson => _loadJson ??= new RelayCommand(async () =>
        {
            Json = await _tokenService.GetTokenAsync();
        });
```





# B站评论总结:

> 文件范围的命名空间简化时，只需要在==命名空间后面写一个分号==，
> VS2022就会自动将格式改为文件范围的命名空间。



> 视频中关于返回不同collection和相同collection的属性写法，其实没有什么特别的，字段和属性的概念搞清楚就不会困惑，第一种是属性缩写写法，是有隐含字段的，属性只是返回字段引用，所以效果是始终返回同一个collection, 第二种写法在类的实现里根本就没有对应字段，每次属性调用都是临时创建一个集合对象返回。所以尽量不要使用属性缩写写法，尽量用propful 写属性完整写法(把内部封装的private字段显示写出来)，就不会困惑了



> 可以利用继承，写一个基类PageViewModelBase，继承ObservableObject，然后在基类里面写常用命令，并指定虚方法；子类只需要重写虚方法就可以了，命令就不用写了，直接用；
> 然后ObservableCollection和List之间的转换确实比较蛋疼，平时我都是用List的Linq做转换，也可以自定义一个扩展方法来处理；list.ToList().ForEach(x => Poetries.Add(x));



> 明年的微服务架构课程就是用标准依赖注入容器了
>
> 用 x:DataType 来标记 Bin
>
> dingContext 的类型，这样就有智能感知了，而且加了这个标记后 Release 编译时会生成 Compiled Bindings，性能比较好





> 往往往低版本迁移就会面临兼容性问题,还是就老老实实都用最朴实的csharp语法，哪怕重复代码多写一点，但是兼容性好，只是看起来low一点。困惑
>
> 采用微服务架构可以部分破解此类问题





> ObservableCollection add这里，多次按键的话好像会重复添加吧。也可以=new(list)来生成个新的ObservableCollection来一次添加所有的



> Sqlite在xamarin下挂掉也不是一次两次了

* 目前官方没有什么呢特别好的方案。民间方案是1.先引入Nuget包WinUIEx 1.5.0版本。
  <PackageReference Include="WinUIEx" Version="1.5.0" Condition="$(【MSBuild】::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'windows'" />

  然后改 MauiProgram

* 在ui上方中部会提示绑定失败
  vs2022提示为
  'Title' property not found on 'MauiApp1.ViewMo...
  https://www.bilibili.com/h5/note-app/view?cvid=18650962&pagefrom=comment&richtext=true

> MAUI里面Gird行与列的定义有个简单的写法：
>
> <Grid RowDefinitions="*,Auto" ColumnDefinitions="*,*,*"></Grid>
> 目前这种写法好像还没更新文档。



> 如何在命令执行的过程中禁用按钮，我现在是用了一个包装类，里面有两个属性，IsEnabled和Icommand,把按钮的IsEnabled绑定到包装类的isEnabled上，运行命令的时候设置包装类isEnabled属性为false,命令执行完了，再恢复过来，不知道有没有更好的实现方式

*  RelayCommand里面有个属性可以直接改变对应绑定控件的执行状态，我记得。可以传递一个Lambda表达式来计算command的可执行状态，同时调用一个什么东西来着，就能让控件自己去读取lambda表达式的值从而更新enable状态



> ListView好像有个很奇怪的bug，在windows端调试点击list按钮刷新绑定时，明明已经绑定了，却提示xaml绑定失败。原因竟然是无法在ViewModel里面找到Title和Context。这个就很奇特，按说前面绑了Poetries，就应该再Poetries里面找。后来我试了一下如果将ObservableCollection改为List，报错能消失。但是只要是ObservableCollection，哪怕在DataTemplate里面将DataType指定为Poetry，报错依然存在。后来我看了看文档，文档上的意思是要用CollectionView替代ListView，我试了一下，这回果然腰不酸了腿不疼了，问题都解决了

* Maui CollectionView的文档里写的是： CollectionView 是一种使用不同布局规范呈现数据列表的视图。 它旨在提供更灵活、更高性能的 ListView替代方法。并且提供了从ListView迁移到 CollectionView 的指南。



> 安卓端闪退问题可以通过引入几个Nuget包解决。代码无需进行更改，我在手机上已经测试了没问题。IOS好像还需要重新设置SQLite provider，由于我没有设备所以实验不了，有土豪看见了这条可以赞助我一台iphone![[doge]](https://i0.hdslb.com/bfs/emote/3087d273a78ccaff4bb1e9972e2ba2a7583c9f11.png@100w_100h.webp)
> nuget包如下：
> 	  <PackageReference Include="SQLitePCLRaw.bundle_green" Version="2.1.1" />
> 	  <PackageReference Include="SQLitePCLRaw.core" Version="2.1.1" />
> 	  <PackageReference Include="SQLitePCLRaw.provider.dynamic_cdecl" Version="2.1.1" />
> 	  <PackageReference Include="SQLitePCLRaw.provider.sqlite3" Version="2.1.1" />
