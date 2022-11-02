using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BaseApiApp.Entities.Helper
{
    public class Helper
    {

        #region(PASSWORD ENCRYPT/DECRYPT)
        public static string EncryptPasswordSha(string password)
        {
            SHA256CryptoServiceProvider shaHasher = new SHA256CryptoServiceProvider();
            byte[] hashedDataBytes;
            UTF8Encoding encoder = new UTF8Encoding();
            hashedDataBytes = shaHasher.ComputeHash(encoder.GetBytes(password));
            System.Text.StringBuilder strHex = new System.Text.StringBuilder();
            foreach (byte b in hashedDataBytes)
            {
                strHex.Append(b.ToString("x2").ToLower());
            }
            string str = strHex.ToString();
            return str;
        }
        #endregion

        #region(GENERATECOUPON)
        public static string GenerateCoupon(int length)
        {
            Random random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            LoginX._rndomcode = result.ToString();
            return result.ToString();
        }

        public static class LoginX
        {
            public static string _rndomcode { get; set; }
        }

        string sValue = LoginX._rndomcode;
        #endregion
    }
}