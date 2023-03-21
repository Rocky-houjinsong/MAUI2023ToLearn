MAUI 如何进行单元测试 ;

首选xUnit测试项目,其次是TUnit;

MVVM + IService ;

MVC 很好理解,是功能的划分;

Service是 依赖注入到ViewModel的; ;

依赖注入的方式,此前的Xamarin 借助 SimpleIoc进行依赖注入;

MAUI 通过 官方的MVVMToolkit 的 GetServices进行依赖注入;

# 单元测试

---

## 测试约定

> * 测哪个类,就把对应的文件路径在测试项目中 对应起来, 说人话就是 , 建文件夹 ;
> * 给测试函数命名 : 被测+ 测试条件
> * [Fact] 标记表明该函数是测试函数

---

测试对象 ,一般直接 new出来; 

* `测试选项卡` 打开 `测试资源管理器`

主项目 和  测试项目的  独立的项目, 虽然同属于 一个解决方案下面;
需要 引用  被盗用的资源项目;  测试项目, 需要引用被测试项目;

* `添加项目引用`  ==此时在`依赖项` 上面出现黄色异常警告==   :interrobang:  为何如此?

> 尝试 运行 ,发现报错 

MAUI 单项目架构;

:interrobang:  :warning:  **为何MAUI无法单元测试,而控制台项目可以测试?**

> * 双击 查看 Project信息;
>
> 主项目是 <TargetFrameworks>net6.0-android;net6.0-ios;net6.0-maccatalyst</TargetFrameworks>
> 测试项目是 <TargetFramework>net6.0</TargetFramework>
>
> 你引用主项目 之后,测试项目 报错,  无法识别平台,主项目 和 测试项目的平台**是不兼容的;** 
> 而 ==创建的 控制台项目 及其 测试项目 是 兼容的==   ,而 `Xamarin`  的是兼容的,没有这个问题;

:key: 解决方法 : 将要测试的项目代码部分,从不能测试的平台中,剥离出来,以类库的方式存在; 
==稍微复杂的项目 ,都是 将 核心功能模块 ,以类库的形式,被 主项目, 测试项目进行引用;==

**C++,Rust 也可以 以类库的形式存在,让C#项目调用,提升性能,扩展功能**

**步骤:**

1. 在 解决 方案`Solution`上,右键`添加新项目` , 筛选` 库` ,选择`类库`
   命名`DailyPoetryM.Library`, 框架选择 `.NET6.0`   ;创建完毕;

2. 修改项目 :**自带命名空间为  **DailyPoetryM.Library ,需要统一为 `DailyPoetry`
   ==修改项目的 默认命名空间==

   > 1. 类库项目上,点右键, 选择`属性`
   > 2. 修改 `默认命名空间` 为 `DailyPoetryM`  ,回车
   > 3. 配置文件变为 <RootNamespace>DailyPoetryM</RootNamespace>  ,说明 修改成功
   > 4. 删掉 默认的 `class1`文件;

3. 在类库中,将需要测试的项目代码,重新的新建 和熟悉,  ==不要直接拖动/复制粘贴==



:warning:  `Preference`  是 MAUI框架的一部分, 这个 不好剥离,如何再测试呢?
:interrobang:  

:key: 

* 使用接口 来完成, 新建`IPreferenceStorage`接口
  它的本质上  `键值存储Key-Value` ,自己 添加新的接口`IPreferenceStorage`;

* 让 `_preferenceStorage` 替换 `PreferenceStorage` 

* 依赖注入,以 构造函数的方式,依赖注入 ,生成该属性值;

  ```c#
  //*******************************私有变量  
  private readonly IPreferenceStorage _preferenceStorage;
  
      //***********************构造函数
      public PoetryStorage(IPreferenceStorage preferenceStorage)
      {
          _preferenceStorage = preferenceStorage;
      }
  
      /// <summary>
      /// 判断诗词数据库是否初始化.
      /// </summary>
      /// <remarks>比较偏好存储中和 版本信息中的值;</remarks>
      public bool IsInitialized =>
          _preferenceStorage.Get(PoetryStorageConstant.VersionKey, 0) == PoetryStorageConstant.Version;
  
  ```

  

4. 测试项目,添加 类库引用;

> 项目选择
>
> * O2O 外卖平台,需要服务器, 数据假的,细节能学到,但是 行不通
> * UGC(用户生成内容) 论坛,视频网站等, 难的是监管;
> * 以服务,数据为中心的 ,慕课,百科, 新闻阅读器;; 服务器, 

从身边的问题和需求出发; 

## 开始测试

----

> 01: new 出一个 `PoetryStorage` ,但是 构造方法 需要传入 一个  `IPreferenceStorage` 

必须 给 一个 `实现` 的 `IPreferenceStorage` ,要是手写实现,还是底层的MAUI的 `Preference` 

:interrobang: **解决测试里面 不能或还没实现 的接口,如何模拟?**

:key:  使用包 `Moq`  来 ==造实例== , ==控制实例的行为==

:star: 如何控制行为 ?  `SetUp` 



# IsInitialized_Default

> 测试初始化判定

```c#
 var preferenceStorageMock = new Mock<IPreferenceStorage>();    //Mock工具
        preferenceStorageMock.Setup(p => p.Get(PoetryStorageConstant.VersionKey, 0))
            .Returns(PoetryStorageConstant.Version);
        var mockPreferenceStorage = preferenceStorageMock.Object;  //mock出的对象
        var poetryStorage = new PoetryStorage(mockPreferenceStorage);
```



> 02: 调用poetrystorage的初始化方法

* 造实例的时候  ,Version 就是返回值,为true



# :star: TestInitializeAsync_Default

>  调用初始化,判定 版本号信息就可以
> 此时不需要返回值,就不需要 使用 `SetUp`方法 

* 如何验证 文件是否拷贝成功?
  * 文件不存在,--->拷贝, ----->文件存在

> :warning:  此时报错, 进行断点调试
>
>  Assert.False() Failure
> Expected: False
> Actual:   True

```c#
Assert.False(File.Exists(PoetryStorage.PoetryDbPath));

```

* 断点调试 ,查看 文件路径  ----> 文件创建,但大小为空

  * 中断调试, 手动删除 ,再调试;出现错误
    此时报错

    ` System.NullReferenceException : Object reference not set to an instance of an object.`  

  * 再次调试, 报最初的错误;  ---------------> 所以这步是没有问题的;需要测试 初始化函数,进行调试查看

* 发现是 `await dbAssetStream.CopyToAsync(dbFileStream);` 中的 `dbAssetStream` 为 null;
  ==资源就是文件名??==  , 
  <font color = gree size> 即时窗口</font> : 断点中,程序运行处进行查看

> 断点打在 `await dbAssetStream.CopyToAsync(dbFileStream);` 
>
> 删除 文件路径的 `C:\Users\hp\AppData\Local\Sqlite3`文件
>
> 调试
>
> 打开即时窗口,依次输入: typeof(PoetryStorage)   typeof(PoetryStorage).Assembly 都没问题
> typeof(PoetryStorage).Assembly.GetManifestResourceStream(DbName)  返回Null;
>
> ==输入typeof(PoetryStorage).Assembly.GetManifestResourceNames()==  
>
> 返回  [0]: "DailyPoetryM.poetrydb.sqlite3"   

:key: 所以说 , 问题出现在 , 资源名 ,不是 文件名, 前面添加了 命名空间;
解决方法: :star: 

1. 直接使用该 完整的 文件名;
2. 让资源名 直接就使用我的文件名;---------------- >==看英文 , GitHub搜做法==,   <font color = red> 多去看别人怎么做的</font>

```xaml
 <!--类库项目,双击,打开配置文件, 修改嵌入式资源 -->
<!--原来-->
 <ItemGroup>
    <EmbeddedResource Include="poetrydb.sqlite3" />
  </ItemGroup>
<!--现在-->
 <ItemGroup>
	  <EmbeddedResource Include="poetrydb.sqlite3">
		  <LogicalName>poetrydb.sqlite3</LogicalName>
	  </EmbeddedResource>
  </ItemGroup>
```

> 此时, 需要先将文件删除,再次运行就能成功;
>
> 但是 ,第二次运行就失败; ==单元测试会生成垃圾文件,影响测试环境== 
>
> :key:  `原则` 测试 之后,打造环境  
> 单元测试之后 打扫, 测试之前 也打扫;



单元测试 提供了 机制 辅助完成;

单元测试之前运行, 放在构造函数里面;

单元测试之后运行,放在析构函数里面;------------> 深入到GC里面; 

而 NUnit 使用标记`Setup,TearDown` 来解决 ; 单元测试的流派区别;



------

> 下面 接着测试: 测试 初始化之后  是否成功

* 前者 通过SetUP 调用方法,的返回值进行判定; 从调用返回值验证
* 后者通过Verify  是否调用,且调用的次数;   从调用次数验证  

将资源导入到 测试项目中,再测试,通过;  

<font color = gree size = 5>总结与反思</font>

> 本次是测试 辅助找到 代码的错误, 直观原因是 ,以为 资源就是文件名;
> :star: **如何解决和避免这类问题呢?** -------------------==防御性编程==;
>
> 1. 知道自己在踩坑;
> 2. 抛异常, 返回空值------------ 了解基本原因;  ==主动抛出异常==;  -----------> 修改代码,添加异常处理机制; 



# GetPoetryAsync_Default

> 获取一首诗词 的数据;
> 因为获取需要在连接数据库之后才有的操作, 单元测试文件中 测试所有的操作之后会清除环境,
>
> 所以 **获取**之前,自己在测试方法内调用**初始化**方法;
> **验证**

---

视频中 出错,但自己照着到现在, 测试通过;
==无法访问文件,IO错误,被其他应用程序占用==



> 考虑, 作为诗词小软件,  运行期间,有没有必要 关闭数据库呢?
> 单机小程序,没必要关闭,因为无法预测 用户何时使用 ;
>
> 当用户 退出程序, 数据库 自然也就关掉;
> 从业务角度讲解, 不需要关闭;
>
> 但是 ,从 单元测试角度上讲, 不关闭数据库,单元测试就过不了;

<font color = red> 接口关注的是业务的需求,实现类可以定义方法解决测试的需求</font>

<font color = gree > 做软件, 需求分析--> 设计--> 实现 ;   中间的设计,不单纯受到需求的影响,也会受到实践的影响</font>



> 同时并行测试 ,有可能会失败 ;
> 是同时生成4个实例;
> Nunit 是 单线程, xUnit是多线程;
> 如何屏蔽多线程呢?

在测试项目上添加文件`xunit.runner.json`  

```json
{
	"parallelizeAssembly":false,
	"parallelizeTestCollections":false
}
```





# GetPoetriesAsync_Default

---

获取全部的 诗词 数据

* 重点在于理解  Lambda 表达式的创建 含义

  ```c#
  var poetries = await poetryStorage.GetPoetriesAsync(
              Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true),
                  Expression.Parameter(typeof(Poetry), "p")), 0, int.MaxValue);
          // Connection.Table<Poetry>.Where(p => true)
          //select * from Poetry Where TRUE
  ```

  

# **B站评论**



> 怎么才能创建一个只生成一个目标平台的配置呢？这样我反复调试的时候可以省点时间。

* 通过改.csproj文件实现。例如只运行于windows的话，将TargetFrameworks注释掉，SupportedOSPlatformVersion里面其他平台注释掉即可

> 封装接口和Mock

* 单元测试方法和代码一直没有变。
  测试项目和lib即使标注了UseMaui，真正调用Maui的东西也只是不会在生成时报错，运行的时候依然不能用。
  不过单元测试有时候也不能完全覆盖，比如面对一些工业控制或工业自动化的业务时，如果是使用串口等硬件与真实设备进行复杂流程交互!
  不过令人欣慰的是这种硬件程序迭代较慢，一般发布了不会大改



> 单元测试保证代码质量、快速定位问题，
> 而Dependency Injection完完全全是为单元测试服务的



> 单元测试项目是引用MAUI项目会报错，不过这个可以通过更改.csproj...  **来自笔记 需要补充**

* 核心业务部分其实完全可以做到与前端显示部分分离，而且单独封装到一个类库项目里。这种结构就和很多复杂项目比较接近了
* 分门别类放在类库里面，其实好像只要这个类库如果写了<UseMaui>true</UseMaui>的话，理论好像也是应该能用Preferences，回头我再试试。
* 各大云服务商都会有免费（或极低价格）的学生服务器
* 把mvvm和services移动到类库项目，然后类库项目和测试项目csproj文件中PropertyGroup增加UseMaui就可以进行测试了。MAUI主项目引用类库项目即可，无需再更改csproj文件



------

*  VS自带的代码模板

* MAUI简单的行列定义方法

* ListView 过期,被废弃,使用CollectionView;

* 代码生成器,使用新版本的 `CommunityToolkit` 生成Parital位置

  * 修改项目的配置文件  

    :star: 
    在`PropertyGroup节`中 新增 <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles> 
    ==这个属于谷歌的东西,不是.NET,体系里的==
    **输出生成的文件到obj文件夹**

* 依赖注入 不是 MVVM所必须的,但是整体更好

* 是否使用语言高级特性,  使用,迁移性低;

* servicelocator模式 ,项目使用的  同一个视图的单个实例; 之后 微服务架构会讲解 多实例的模式;
  ==指定datatype 获得编译时提示==



> MAUI的话推荐==Syncfusion的控件库==，这个官网注册下，针对个人使用和微型团队
> （5人以下，年收入不超过100万刀）提供了免费申请终身使用的社区许可，是免费使用的；
> UI，图表都有，官方文档也比较全，MAUI刚出来的时候，就开始更新MAUI控件库

* WinUI 3 Gallery 微软商店搜索就可以下载，官方的控件库。功能还蛮多的

> 一些第三方nuget适配还是有一定问题的，本体问题倒不大

> CommunityToolkit.Maui包不支持.Net 6？UseMauiCommunityToolkit()不能用

* CommunityToolkit.Maui 3.0开始不支持.Net 6，要选2.0版

> 跨页面传参也是传属性，用的是QueryProperty。**把IEnumerable变成List的原因**

* IEnumerable和IQueryable是可枚举接口，List是默认实现。
  数据类型在ORM里有重要的指示作用，接口表示一个正在构建的数据查询命令，可能随着程序运行继续修改。
  调用ToList则表示命令已经构建完毕，可以编译命令并发送到底层存储调取数据。
  这在内存查询中也是实现延迟执行和流式处理的关键技术。
  如果你去看EF Core文档就会看到3.1版本的重大更改就是自动客户端查询被弃用，
  这个功能就和类型指示有深刻关系并且隐藏暗坑，所以这个自动化功能被弃用，
  要求开发者明确选择查询方式，框架不再自作主张

# 个人学习

> NuGet包, 安装到其他盘;

> 测试

* 不可测试的项目代码 ,剥离到可测试的项目中
* 不可测试的部分, 剥离成接口 
* 做测试的时候, 使用Mock工具 实例化接口,变为可测试;



> 测试是 非常重要的 !!!! 

* 测试意味着  保护你的代码;
* 测试保护 我的代码是安全可靠的;不可被别人修改;

