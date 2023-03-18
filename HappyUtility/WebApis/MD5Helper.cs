using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HappyUtility.WebApis
{
    public static class MD5Helper
    {
        public static string ToMD532(this string str)
        {
            return MD5Encrypt32(str);
        }
        public static string MD5Encrypt32(string password)
        {
            var pwd = new StringBuilder();
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < s.Length; i++)
            {
                pwd.Append(s[i].ToString("X"));
            }
            return pwd.ToString();

        }
    }
}
