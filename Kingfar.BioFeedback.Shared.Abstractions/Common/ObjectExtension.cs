using System.Security.Cryptography;
using System.Text;

namespace Kingfar.BioFeedback.Shared
{
    public static class ObjectExtension
    {
        public static string GetMd5Hash(this string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // 将每个字节转换为16进制字符串
                }

                return sb.ToString();
            }
        }
    }
}