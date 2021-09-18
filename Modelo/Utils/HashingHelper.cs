using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Model.Utils
{
    public static class HashingHelper
    {
        public static string ComputeSha256Hash(string str)
        {
            using (var sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(str));
                var builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }


                return builder.ToString();
            }
        }

        public static int GenerateRandomNo()
        {
            int _min = 0000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
    }
}