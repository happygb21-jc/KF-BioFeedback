using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Kingfar.BioFeedback.WPFUI
{
    public class StringEmptyValidationRule : ValidationRule
    {
        public string Title { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return new ValidationResult(false, $"{Title}不能为空");
            }
            else
            {
                if (Title == "身份证号" && !ValidateIdNumber(value?.ToString()))
                {
                    return new ValidationResult(false, $"{Title}不正确");
                }
                if ((Title == "手机号" || Title == "联系方式") && !ValidatePhoneNumber(value?.ToString()))
                {
                    return new ValidationResult(false, $"{Title}不正确");
                }
            }

            return ValidationResult.ValidResult;
        }

        public bool ValidatePhoneNumber(string phoneNumber)
        {
            // 使用正则表达式验证手机号码
            string pattern = @"^1[3456789]\d{9}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(phoneNumber);
        }

        // 验证身份证号码
        public bool ValidateIdNumber(string idNumber)
        {
            // 使用正则表达式验证身份证号码
            string pattern = @"^\d{17}(\d|X|x)$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(idNumber);
        }
    }
}