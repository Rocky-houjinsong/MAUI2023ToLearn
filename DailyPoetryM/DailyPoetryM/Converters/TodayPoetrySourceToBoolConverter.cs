using System.Globalization;
using DailyPoetryM.Misc;

namespace DailyPoetryM.Converters;

// 诗词来源,本地返回false
public class TodayPoetrySourceToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
    {
        #region 理解

        // value 不是字符串,直接返回;   
        // source 来源网络,返回true, 本地 返回false
        //这个写法 很过分了
        // && 短路特性 value 不是字符串,直接返回 , 是字符串,就存储到 Source中;
        // parameter 是期望值 , 期望值也得是字符串; 
        // 作用 : 传过来的值 ,和传过来的转换器参数是否匹配

        #endregion

        return value is string source && parameter is string expectedSource &&
               source == expectedSource;
    }

    public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture) =>
        throw new DoNotCallThisException();
}