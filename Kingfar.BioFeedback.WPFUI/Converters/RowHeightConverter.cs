using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Kingfar.BioFeedback.WPFUI.Converters
{
    public class RowHeightConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double rowActualHeight = (double)values[0];
            double dataGridActualHeight = (double)values[1];

            // 根据需要进行适当的计算
            double desiredHeight = rowActualHeight - 250; // 示例中减去 50

            return desiredHeight;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}