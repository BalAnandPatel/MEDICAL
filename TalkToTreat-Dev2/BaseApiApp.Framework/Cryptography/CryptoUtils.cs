using System; 
using System.Text;
using Security.Cryptography;
namespace BaseApiApp.Framework.Cryptography
{
    public class CryptoUtils
    {

        #region "Generate random AlphaNum string"
        /// <summary>
        /// Returns a random alphanumeric string generated using RNGCNG
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GenerateRandom(int RndLength)
        {
            string rndChar = null;
            RNGCng rng = new RNGCng();
            int byteArrLength = 0;

            //if we want a 10 char random no. then byte array has to be 4 and hence the following formula
            byteArrLength = (RndLength / 2) - 1;

            byte[] randomBytes = new byte[byteArrLength + 1];
            rng.GetBytes(randomBytes);
            rndChar = BitConverter.ToString(randomBytes);

            return (rndChar);
        }
        /// <summary>
        /// Returns a random alphanumeric string generated using RNGCNG and also generates the SHA hash for the same
        /// </summary>
        /// <param name="RndLength">Lenght of the random string</param>
        /// <param name="encryptedRandom">Passed by reference</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GenerateRandom(int RndLength, ref string encryptedRandom)
        {
            string rndChar = null;
            RNGCng rng = new RNGCng();
            int byteArrLength = 0;

            //if we want a 10 char random no. then byte array has to be 4 and hence the following formula
            byteArrLength = (RndLength / 2) - 1;

            byte[] randomBytes = new byte[byteArrLength + 1];

            rng.GetBytes(randomBytes);
            rndChar = BitConverter.ToString(randomBytes);


            //--the Salt is passed as Blank because when we will be validating the Voucher Secret code we will comapre both the Hashes 
            //--and we will not know the right Salt, hence we have kept the Salt as "" for Secret Codes

            //--Generate the encrypted hash for random string
            CryptoSHA crypto = new CryptoSHA();
            //encrypt the pwd
            encryptedRandom = crypto.Encrypt(rndChar, "");
            //--2nd time encryption
            encryptedRandom = crypto.Encrypt(encryptedRandom, "");

            return (rndChar);
        }

        #endregion

        public static string GenerateSHAHash(string clearText, string Salt)
        {
            string encryptedText = null;
            //--Generate the encrypted hash for random string
            CryptoSHA crypto = new CryptoSHA();
            //encrypt the pwd
            encryptedText = crypto.Encrypt(clearText, Salt);
            //--2nd time encryption
            encryptedText = crypto.Encrypt(encryptedText, Salt);

            return encryptedText;
        }

        #region "Generate Unique"
        /// <summary>
        /// using Microsoft Cryptographic API Next Generation (CNG) Random Number Generator and MD5 hash algorithm as well.
        /// </summary>
        /// <param name="RandomBytesSize"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        /// <history>Yevgen Ryeznikov | 4.Oct.2011</history>
        public static string GetUniqueKey(int RandomBytesSize)
        {
            if (RandomBytesSize >= 6)
            {
                //YRY: At least 16 real random bytes from CNG provider
                byte[] randomBytes = null;
                randomBytes = GetRandomByteArray(RandomBytesSize);

                //YRY: At least 18 bytes of unique timestamp; Tick ~ 100 Nanoseconds
                byte[] timestampBytes = Encoding.Default.GetBytes(DateTime.Now.Ticks.ToString());

                //YRY: Merge bytes to one array: Randomness + Uniqueness
                int Size = RandomBytesSize + timestampBytes.Length;
                byte[] UIDBytes = new byte[Size];
                System.Buffer.BlockCopy(randomBytes, 0, UIDBytes, 0, randomBytes.Length);
                System.Buffer.BlockCopy(timestampBytes, 0, UIDBytes, randomBytes.Length, timestampBytes.Length);

                //YRY: Create 16 byte unique MD5 hash of merged array (34 Bytes)
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] md5OfUIDBytes = md5.ComputeHash(UIDBytes);

                //YRY: Convert MD5 to alphanumeric string
                char[] chars = new char[36];
                //chars = "abcdefghijklmnopqrstuvwxyz1234567890".ToCharArray();
                //YRY: I dont take "zero" because its too similar to "O"
                //excluding I/1/L/l + S/5
                chars = "2346789ABCDEFGHJKMNOPQRTUVWXYZ".ToCharArray();

                StringBuilder result = new StringBuilder(Size);
                foreach (byte b in md5OfUIDBytes)
                {
                    result.Append(chars[b % chars.Length]);
                }

                //return Convert.ToBase64String(md5OfUIDBytes);
                return result.ToString();
            }
            else
            {
                throw new Exception("Min random byte size must be 16 chars");
            }
        }
        public static byte[] GetRandomByteArray(int Size)
        {
            byte[] RBA = null;
            if (Size > 0)
            {
                RBA = new byte[Size];
                RNGCng myRNG = new RNGCng();
                //myRNG.GetNonZeroBytes(RBA); //not implemented in MS CNG standard provider
                myRNG.GetBytes(RBA);
            }
            else
            {
                RBA = null;
            }
            return RBA;
        }

        #endregion
        /// <summary>
        /// gets the Unix time stamp for the current date / time.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double GetUnixTimestamp()
        {
            DateTime date = System.DateTime.UtcNow;

            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}
