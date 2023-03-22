using System.Globalization;
using DailyPoetryM.Misc;

namespace DailyPoetryM.Converters;

public class NegativeBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
    {
        if (value is bool b)
        {
            return !b;
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture) =>
        throw new DoNotCallThisException();
}