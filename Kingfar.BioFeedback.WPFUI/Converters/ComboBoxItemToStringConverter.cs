using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Controls;

namespace Kingfar.BioFeedback.WPFUI.Converters
{
    public class ComboBoxItemToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ComboBoxItem comboBoxItem)
            {
                return comboBoxItem.Content?.ToString();
            }

            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
