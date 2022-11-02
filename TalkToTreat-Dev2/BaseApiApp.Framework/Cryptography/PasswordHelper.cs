using System;
using System.Text;
using Microsoft.VisualBasic;
namespace BaseApiApp.Framework.Cryptography
{
   public class PasswordHelper
    {
       
	//Private Shared characterArray() As Char

	private static char[] characterArray = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
    public PasswordHelper()
	{
		characterArray = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
	}

	private static char GetRandomCharacter()
	{
        VBMath.Randomize();
		Int32 location = -1;
		while (!(location >= 0 && location <= characterArray.GetUpperBound(0))) {
			location = Convert.ToInt32(characterArray.GetUpperBound(0) * VBMath.Rnd() + 1);
		}

		return characterArray[location];
	}

	public static string Generate()
	{
		Int32 count = default(Int32);
		StringBuilder sb = new StringBuilder();
		Int32 passwordLength = 10;

		sb.Capacity = passwordLength;
		for (count = 0; count <= passwordLength - 1; count++) {
			sb.Append(GetRandomCharacter());
		}
		if (((sb != null))) {
			return sb.ToString();
		}
		return string.Empty;
	}
	/// <summary>
	/// Generates a random password and encrypts it using Microsoft SHA Crypto
	/// </summary>
	/// <param name="Salt">passed by reference so that it can be saved in db by the caller function</param>
	/// <returns></returns>
	/// <remarks></remarks>
	public static string GenerateAndEncrypt(ref string Salt)
	{
		Int32 count = default(Int32);
		StringBuilder sb = new StringBuilder();
		string encryptedPwd = null;
		Int32 passwordLength = 10;

		sb.Capacity = passwordLength;
		for (count = 0; count <= passwordLength - 1; count++) {
			sb.Append(GetRandomCharacter());
		}
		if (((sb != null))) {
			encryptedPwd = sb.ToString();
			CryptoSHA crypto = new CryptoSHA();
			encryptedPwd = crypto.Encrypt(encryptedPwd, "");
			//--generate salt
			Salt = CryptoUtils.GetUnixTimestamp().ToString();
			//encrypt the pwd

			//--TODO: need to change the above line to below line:
			//encryptedPwd = crypto.Encrypt(encryptedPwd, "")
			//--2nd time encryption
			encryptedPwd = crypto.Encrypt(encryptedPwd, Salt);

			return encryptedPwd;
		}
		return string.Empty;
	}

	/// <summary>
	///  encrypts the password  using Microsoft SHA Crypto
	/// </summary>
	/// <param name="Salt">passed by reference so that it can be saved in db by the caller function</param>
	/// <returns></returns>
	/// <remarks></remarks>
	public static string EncryptPassword(ref string Salt, string password)
	{
		string encryptedPwd = null;
		if (((password != null))) {
			encryptedPwd = password;
			CryptoSHA crypto = new CryptoSHA();
			//--generate salt
			if ( string.IsNullOrEmpty(Salt)) {
				Salt = CryptoUtils.GetUnixTimestamp().ToString();
			}
			//encrypt the pwd
			encryptedPwd = crypto.Encrypt(encryptedPwd, "");
			//--TODO: need to change the above line to below line:
			//encryptedPwd = crypto.Encrypt(encryptedPwd, "")
			//--2nd time encryption
			encryptedPwd = crypto.Encrypt(encryptedPwd, Salt);

			return encryptedPwd;
		}
		return string.Empty;
	}
    }
}
