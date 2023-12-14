using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Kingfar.BioFeedback.WPFUI.Converters
{
    public class RgbaToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string rgbaValue)
            {
                string[] rgbaComponents = rgbaValue.Split(',');

                if (rgbaComponents.Length == 4 &&
                    byte.TryParse(rgbaComponents[0], out byte r) &&
                    byte.TryParse(rgbaComponents[1], out byte g) &&
                    byte.TryParse(rgbaComponents[2], out byte b) &&
                    byte.TryParse(rgbaComponents[3], out byte a))
                {
                    return Color.FromArgb(a, r, g, b);
                }
            }

            return Colors.Transparent; // 默认返回透明色
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
