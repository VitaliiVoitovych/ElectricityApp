using System.Globalization;

namespace ElectricityApp.Converters;

public class DateOnlyConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is DateOnly date) return new DateTime(date.Year, date.Month, date.Day);

        return DateTime.Now;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return DateOnly.FromDateTime((DateTime)value!);
    }
}
