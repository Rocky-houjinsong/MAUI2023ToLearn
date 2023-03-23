using System.Globalization;
using DailyPoetryM.Misc;

namespace DailyPoetryM.Converters;

public class NegativeBoolConverter : IValueConverter
{
    //布尔取反
    public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
    {
        /*if (value is bool b)
        {
            return !b;
        }

        return null;*/
        return value is bool b ? !b : null;
    }


    public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture) =>
        throw new DoNotCallThisException();
}