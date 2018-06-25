using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AutoLead
{
	public class EncryptedString
	{
		public EncryptedString()
		{
		}

		public static string DecryptString(string base64StringToDecrypt, string passphrase)
		{
			string end;
			using (AesCryptoServiceProvider provider = EncryptedString.GetProvider(Encoding.Default.GetBytes(passphrase)))
			{
				byte[] numArray = Convert.FromBase64String(base64StringToDecrypt);
				ICryptoTransform cryptoTransform = provider.CreateDecryptor();
				MemoryStream memoryStream = new MemoryStream(numArray, 0, (int)numArray.Length);
				CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read);
				end = (new StreamReader(cryptoStream)).ReadToEnd();
			}
			return end;
		}

		public static string EncryptString(string plainSourceStringToEncrypt, string passPhrase)
		{
			string base64String;
			using (AesCryptoServiceProvider provider = EncryptedString.GetProvider(Encoding.Default.GetBytes(passPhrase)))
			{
				byte[] bytes = Encoding.ASCII.GetBytes(plainSourceStringToEncrypt);
				ICryptoTransform cryptoTransform = provider.CreateEncryptor();
				MemoryStream memoryStream = new MemoryStream();
				CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
				cryptoStream.Write(bytes, 0, (int)bytes.Length);
				cryptoStream.FlushFinalBlock();
				base64String = Convert.ToBase64String(memoryStream.ToArray());
			}
			return base64String;
		}

		private static byte[] GetKey(byte[] suggestedKey, SymmetricAlgorithm p)
		{
			byte[] numArray = suggestedKey;
			List<byte> nums = new List<byte>();
			for (int i = 0; i < p.LegalKeySizes[0].MinSize; i += 8)
			{
				nums.Add(numArray[i / 8 % (int)numArray.Length]);
			}
			return nums.ToArray();
		}

		private static AesCryptoServiceProvider GetProvider(byte[] key)
		{
			AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider()
			{
				BlockSize = 128,
				KeySize = 128,
				Mode = CipherMode.ECB,
				Padding = PaddingMode.PKCS7
			};
			aesCryptoServiceProvider.GenerateIV();
			aesCryptoServiceProvider.IV = new byte[16];
			aesCryptoServiceProvider.Key = EncryptedString.GetKey(key, aesCryptoServiceProvider);
			return aesCryptoServiceProvider;
		}
	}
}