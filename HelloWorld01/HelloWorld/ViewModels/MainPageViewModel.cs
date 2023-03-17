using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HelloWorld.Models;
using HelloWorld.Services;

namespace HelloWorld.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        #region 数据绑定:View联系ViewModel

        private string _result = string.Empty;

        public string Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        private RelayCommand _clickMeCommand;

        public RelayCommand ClickMeCommand =>
            _clickMeCommand ??= new RelayCommand(() => Result = "Hello World! MAUI");

        #endregion

        #region 键值存储:MVVM+IService架构下的读写键值操作

        private IKeyValueStorage _keyValueStorage;
        private RelayCommand _readCommand;
        private RelayCommand _writeCommand;

        /// <summary>
        /// 读取,并赋值给Result属性
        /// </summary>
        public RelayCommand ReadCommand =>
            _readCommand ??=
                new RelayCommand(() =>
                    Result = _keyValueStorage.Get("MyKey", string.Empty));

        public RelayCommand WriteCommand =>
            _writeCommand ??=
                new RelayCommand(() => _keyValueStorage.Set("MyKey", Result));

        #endregion

        #region 构造函数

        //***********************构造函数
        // /// <summary>
        // /// 构造函数
        // /// </summary>
        // /// <remarks>构造函数的方式进行依赖注入,将ViewModel所必须的Service关联起来</remarks>
        // /// <param name="keyValueStorage">键值存储</param>
        // public MainPageViewModel(IKeyValueStorage keyValueStorage)
        // {
        //     _keyValueStorage = keyValueStorage;
        // }

        /// <summary>
        /// 显式声明 默认构造函数
        /// </summary>
        /// <remarks>数据绑定案例使用的是 默认构造函数, 由于编写带参构造函数,默认的需要显式声明</remarks>
        // public MainPageViewModel()
        // {
        // }

        //*****************数据库,诗词存储需要的
        private IPoetryStorage _poetryStorage;

        public MainPageViewModel(IPoetryStorage poetryStorage)
        {
            _poetryStorage = poetryStorage;
        }


        private ITokenService _tokenService;

        public MainPageViewModel(IPoetryStorage poetryStorage, ITokenService tokenService)
        {
            _poetryStorage = poetryStorage;
            _tokenService = tokenService;
        }

        #endregion

        #region 工业化开发:简化

        /*
         *关闭MainPage.xaml中的 相关代码;
         */

        // [ObservableProperty] private string _result01; //此时 需要将类变为分部类
        //
        // [RelayCommand]
        // private void ClickMe01()
        // {
        //     Result01 = "工业化开发";
        // }

        #endregion

        #region 数据库访问:诗词存储

        /// <summary>
        /// 诗词集合
        /// </summary>
        /// <remarks>只读属性, </remarks>
        public ObservableCollection<Poetry> Poetries { get; set; } =
            new ObservableCollection<Poetry>();

        public ObservableCollection<Poetry> Poetries2
        {
            get => new ObservableCollection<Poetry>();
        }

        public ObservableCollection<Poetry> Poetries3 =>
            new ObservableCollection<Poetry>();
        //**********************Command命令集合

        private RelayCommand _initializeCommand;

        public RelayCommand InitializeCommand =>
            _initializeCommand ??= new RelayCommand(async () => { await _poetryStorage.InitializeAsync(); });

        private RelayCommand _addCommand;

        public RelayCommand AddCommand =>
            _addCommand ??= new RelayCommand(async () =>
            {
                await _poetryStorage.AddAsync(
                    new Poetry
                    {
                        Title = "Title",
                        Content = "Content"
                    });
            });

        private RelayCommand _listCommand;

        public RelayCommand ListCommand =>
            _listCommand ??= new RelayCommand(async () =>
            {
                Poetries.Clear();
                var listAsync = await _poetryStorage.ListAsync();
                foreach (var poetry in listAsync)
                {
                    Poetries.Add(poetry);
                }
            });


        private RelayCommand _deleteCommand;

        public RelayCommand DeleteCommand =>
            _deleteCommand ??= new RelayCommand(async () => { await _poetryStorage.DeleteAsync(); });

        #endregion

        #region Web访问:JSON

        // 可绑定属性
        /// <summary>
        /// Json属性.
        /// </summary>
        public string Json
        {
            get => _json;
            set => SetProperty(ref _json, value);
        }

        /// <summary>
        /// Json属性.
        /// </summary>
        private string _json;

        private RelayCommand _loadJson;

        public RelayCommand LoadJson => _loadJson ??= new RelayCommand(async () =>
        {
            Json = await _tokenService.GetTokenAsync();
        });

        #endregion
    }
}