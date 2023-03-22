using DailyPoetryM.Misc;
using DailyPoetryM.ViewModels;
using System.Globalization;

namespace DailyPoetryM.Converters;

public class ItemTappedEventArgsToPoetryFavoriteConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture) =>
        (value as ItemTappedEventArgs)?.Item as PoetryFavorite;

    public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
    {
        throw new DoNotCallThisException();
    }
}