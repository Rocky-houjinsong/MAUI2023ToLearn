using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <remarks>构造函数的方式进行依赖注入,将ViewModel所必须的Service关联起来</remarks>
        /// <param name="keyValueStorage">键值存储</param>
        public MainPageViewModel(IKeyValueStorage keyValueStorage)
        {
            _keyValueStorage = keyValueStorage;
        }

        /// <summary>
        /// 显式声明 默认构造函数
        /// </summary>
        /// <remarks>数据绑定案例使用的是 默认构造函数, 由于编写带参构造函数,默认的需要显式声明</remarks>
        public MainPageViewModel()
        {
        }

        #endregion

        #region 工业化开发:简化

        [ObservableProperty] private string _result01; //此时 需要将类变为分部类

        [RelayCommand]
        private void ClickMe01()
        {
            Result01 = "工业化开发";
        }

        #endregion
    }
}