using System;
using System.Globalization;
using System.Windows.Data;

namespace RetailCommanderDesktop.Helpers
{
    public class DictionaryKeyToValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IReadOnlyDictionary<string, string> dictionary && parameter is string key)
            {
                if (dictionary.TryGetValue(key, out var result))
                {
                    return result;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
