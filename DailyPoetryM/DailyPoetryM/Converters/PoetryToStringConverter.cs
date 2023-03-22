using System.Globalization;
using DailyPoetryM.Models;

namespace DailyPoetryM.Converters;

internal class PoetryToStringConverter : IValueConverter
{
    /// <summary>
    /// VM传过来的诗词,转为字符串
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        (value is Poetry poetry)
            ? $"{poetry.Dynasty} . {poetry.Author}    {poetry.Snippet}"
            : null;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}