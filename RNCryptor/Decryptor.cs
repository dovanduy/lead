using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RNCryptor
{
	public class Decryptor : Cryptor
	{
		public Decryptor()
		{
		}

		public string Decrypt(string encryptedBase64, string password)
		{
			string str;
			PayloadComponents payloadComponent = this.unpackEncryptedBase64Data(encryptedBase64);
			if (this.hmacIsValid(payloadComponent, password))
			{
				byte[] numArray = base.generateKey(payloadComponent.salt, password);
				byte[] numArray1 = new byte[0];
				AesMode aesMode = this.aesMode;
				if (aesMode == AesMode.CTR)
				{
					numArray1 = base.encryptAesCtrLittleEndianNoPadding(payloadComponent.ciphertext, numArray, payloadComponent.iv);
				}
				else if (aesMode == AesMode.CBC)
				{
					numArray1 = this.decryptAesCbcPkcs7(payloadComponent.ciphertext, numArray, payloadComponent.iv);
				}
				str = Encoding.UTF8.GetString(numArray1);
			}
			else
			{
				str = "";
			}
			return str;
		}

		private byte[] decryptAesCbcPkcs7(byte[] encrypted, byte[] key, byte[] iv)
		{
			string end;
			Aes ae = Aes.Create();
			ae.Mode = CipherMode.CBC;
			ae.Padding = PaddingMode.PKCS7;
			ICryptoTransform cryptoTransform = ae.CreateDecryptor(key, iv);
			using (MemoryStream memoryStream = new MemoryStream(encrypted))
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read))
				{
					using (StreamReader streamReader = new StreamReader(cryptoStream))
					{
						end = streamReader.ReadToEnd();
					}
				}
			}
			return base.TextEncoding.GetBytes(end);
		}

		private bool hmacIsValid(PayloadComponents components, string password)
		{
			bool flag;
			byte[] numArray = base.generateHmac(components, password);
			if ((int)numArray.Length == (int)components.hmac.Length)
			{
				int num = 0;
				while (num < (int)components.hmac.Length)
				{
					if (numArray[num] == components.hmac[num])
					{
						num++;
					}
					else
					{
						flag = false;
						return flag;
					}
				}
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		private PayloadComponents unpackEncryptedBase64Data(string encryptedBase64)
		{
			PayloadComponents array = new PayloadComponents();
			List<byte> nums = new List<byte>();
			nums.AddRange(Convert.FromBase64String(encryptedBase64));
			int length = 0;
			array.schema = nums.GetRange(0, 1).ToArray();
			length++;
			base.configureSettings((Schema)nums[0]);
			array.options = nums.GetRange(1, 1).ToArray();
			length++;
			array.salt = nums.GetRange(length, 8).ToArray();
			length += (int)array.salt.Length;
			array.hmacSalt = nums.GetRange(length, 8).ToArray();
			length += (int)array.hmacSalt.Length;
			array.iv = nums.GetRange(length, 16).ToArray();
			length += (int)array.iv.Length;
			array.headerLength = length;
			array.ciphertext = nums.GetRange(length, nums.Count - 32 - array.headerLength).ToArray();
			length += (int)array.ciphertext.Length;
			array.hmac = nums.GetRange(length, 32).ToArray();
			return array;
		}
	}
}