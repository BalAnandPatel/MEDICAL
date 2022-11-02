using System;

namespace BaseApiApp.Framework.Cryptography
{
    public interface IHashEncryption
    {
        string MicrosoftHash(string password, string salt);
    }
    /// <summary>
    /// Encaosulates the One-way Encryption logic for End User password.
    /// </summary>
    /// <remarks></remarks>
    public class CryptoSHA : IHashEncryption
    {

        public string Encrypt(string password, string salt)
        {

            byte[] PasswordAsByte = null;
            byte[] EncryptedBytes = null;
            string retValue = string.Empty;
            System.Security.Cryptography.SHA512Managed HashTool = new System.Security.Cryptography.SHA512Managed();


            try
            {
                //where getsalt will get the UNIX timestamp
                PasswordAsByte = System.Text.Encoding.Unicode.GetBytes(password + salt);
                EncryptedBytes = HashTool.ComputeHash(HashTool.ComputeHash(HashTool.ComputeHash(PasswordAsByte)));
                retValue = System.Convert.ToBase64String(EncryptedBytes);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if ((EncryptedBytes != null))
                {
                    EncryptedBytes = null;
                }
                if ((PasswordAsByte != null))
                {
                    PasswordAsByte = null;
                }
                if ((HashTool != null))
                {
                    HashTool.Clear();
                }
            }

            return retValue;

        }
        string IHashEncryption.MicrosoftHash(string password, string salt)
        {
            return Encrypt(password, salt);
        }


    }
}
