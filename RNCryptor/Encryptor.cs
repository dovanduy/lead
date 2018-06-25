using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RNCryptor
{
	public class Encryptor : Cryptor
	{
		private Schema defaultSchemaVersion = Schema.V2;

		public Encryptor()
		{
		}

		public string Encrypt(string plaintext, string password)
		{
			return this.Encrypt(plaintext, password, this.defaultSchemaVersion);
		}

		public string Encrypt(string plaintext, string password, Schema schemaVersion)
		{
			base.configureSettings(schemaVersion);
			byte[] bytes = base.TextEncoding.GetBytes(plaintext);
			PayloadComponents payloadComponent = new PayloadComponents()
			{
				schema = new byte[] { (byte)schemaVersion },
				options = new byte[] { (byte)this.options },
				salt = this.generateRandomBytes(8),
				hmacSalt = this.generateRandomBytes(8),
				iv = this.generateRandomBytes(16)
			};
			byte[] numArray = base.generateKey(payloadComponent.salt, password);
			AesMode aesMode = this.aesMode;
			if (aesMode == AesMode.CTR)
			{
				payloadComponent.ciphertext = base.encryptAesCtrLittleEndianNoPadding(bytes, numArray, payloadComponent.iv);
			}
			else if (aesMode == AesMode.CBC)
			{
				payloadComponent.ciphertext = this.encryptAesCbcPkcs7(bytes, numArray, payloadComponent.iv);
			}
			payloadComponent.hmac = base.generateHmac(payloadComponent, password);
			List<byte> nums = new List<byte>();
			nums.AddRange(base.assembleHeader(payloadComponent));
			nums.AddRange(payloadComponent.ciphertext);
			nums.AddRange(payloadComponent.hmac);
			return Convert.ToBase64String(nums.ToArray());
		}

		private byte[] encryptAesCbcPkcs7(byte[] plaintext, byte[] key, byte[] iv)
		{
			byte[] array;
			Aes ae = Aes.Create();
			ae.Mode = CipherMode.CBC;
			ae.Padding = PaddingMode.PKCS7;
			ICryptoTransform cryptoTransform = ae.CreateEncryptor(key, iv);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
				{
					cryptoStream.Write(plaintext, 0, (int)plaintext.Length);
				}
				array = memoryStream.ToArray();
			}
			return array;
		}

		private byte[] generateRandomBytes(int length)
		{
			byte[] numArray = new byte[length];
			(new RNGCryptoServiceProvider()).GetBytes(numArray);
			return numArray;
		}
	}
}