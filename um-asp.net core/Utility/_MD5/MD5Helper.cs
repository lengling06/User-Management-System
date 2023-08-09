using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace um_asp.net_core.Utility._SHA256
{
    public static class MD5Helper
    {
        public static string MD5Encrypt32(string password)
        {
            string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < s.Length; i++)
            {
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;
        }
    }
}
