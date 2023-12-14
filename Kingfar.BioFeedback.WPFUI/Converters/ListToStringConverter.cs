using System.Globalization;
using System.Windows.Data;

namespace Kingfar.BioFeedback.WPFUI.Converters
{
    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<Services.SchemeDto> list)
            {
                if (list.Count <= 3)
                {
                    return string.Join(", ", list.Select(e => e.Name));
                }
                else
                {
                    return string.Join(", ", list.Select(e => e.Name).Take(3)) + ", ...";
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}