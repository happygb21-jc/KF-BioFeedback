using NewLife;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Kingfar.BioFeedback.WPFUI.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool inverse = false;
            if (parameter != null && parameter is string parameterString && parameterString== "Inverse")
            {
                inverse = true;
            }

            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return inverse ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return inverse ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
