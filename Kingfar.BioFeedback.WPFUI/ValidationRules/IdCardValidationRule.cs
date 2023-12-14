using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Kingfar.BioFeedback.WPFUI
{
    public class IdCardValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string idCard = (string)value;
            // 正则表达式用于验证身份证号码
            string pattern = @"^\d{17}(\d|X|x)$";
            if (Regex.IsMatch(idCard, pattern))
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "身份证号码格式不正确");
            }
        }
    }
}