using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Kingfar.BioFeedback.WPFUI.Converters
{
    public class RandomColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                Color backgroundColor = (Color)value;

                // 计算背景色的亮度
                double luminance = (0.299 * backgroundColor.R + 0.587 * backgroundColor.G + 0.114 * backgroundColor.B) / 255;

                // 根据亮度选择文本颜色
                if (luminance > 0.5)
                {
                    return Brushes.Black;
                }
                else
                {
                    return Brushes.White;
                }
            }
            else
            {
                Random random = new Random();
                byte[] rgb = new byte[3];
                random.NextBytes(rgb);

                Color randomColor = Color.FromRgb(rgb[0], rgb[1], rgb[2]);

                return new SolidColorBrush(randomColor);
            }
            return Brushes.Black; // 默认使用黑色文本
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}