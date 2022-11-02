using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BaseApiApp.Framework.Cryptography
{
    public class FIPSHMACSHA512 : HMAC
    {
        public FIPSHMACSHA512(byte[] key)
        {
            this.HashName = "System.Security.Cryptography.SHA512CryptoServiceProvider";
            this.HashSizeValue = 512;
            this.BlockSizeValue = 128;
            this.Key = key;
        }
    }
}
