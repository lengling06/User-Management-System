using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace UM.Utility
{
    public class SHA256Helper
    {
        public static string SHA256(string password) 
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = SHA256Managed.Create().ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++) 
            {
                builder.Append(hash[i].ToString("X2").ToLower());
            }
            return builder.ToString();
        }
    }
}
