using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace BaseApiApp.Framework.Cryptography
{
    
    public class CryptoX509
    { 
 
        public static X509Certificate2 GetCertificate(StoreName name, StoreLocation location, string thumbprint)
        {
            X509Store store = new X509Store(name, location);
            X509Certificate2Collection certificates = null;
            store.Open(OpenFlags.ReadOnly);
            try
            {
                X509Certificate2 result = null;
                // Every time we call store.Certificates property, a new collection will be returned.
                certificates = store.Certificates;
                for (int i = 0; i <= certificates.Count - 1; i++)
                {
                    X509Certificate2 cert = certificates[i];
                    if (cert.Thumbprint.ToLower() == thumbprint.ToLower())
                    {
                        if (result != null)
                        {
                            throw new ApplicationException(string.Format("There is more than one certificate found for thumbprint {0}", thumbprint));
                        }
                        result = new X509Certificate2(cert);
                    }
                }




                if (result == null)
                {
                    throw new ApplicationException(string.Format("No certificate was found for thumbprint {0} on this machine, please use MMC to import the .pfx file to localmachine/personal.", thumbprint));
                }
                return result;
            }
            finally
            {
                if (certificates != null)
                {
                    for (int i = 0; i <= certificates.Count - 1; i++)
                    {
                        X509Certificate2 cert = certificates[i];
                        cert.Reset();
                    }
                }
                store.Close();
            }
        }

    }

}
