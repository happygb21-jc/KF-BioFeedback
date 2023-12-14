using System.Globalization;
using System.Windows.Data;

namespace Kingfar.BioFeedback.WPFUI.Converters
{
    public class BirthdayToAgeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime birthDate)
            {
                int age = CalculateAge(birthDate);
                //return $"出生日期: {birthDate.ToString("yyyy-MM-dd")}, 年龄: {age}岁";
                return $"{age}岁";
            }

            return "0岁";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }
}