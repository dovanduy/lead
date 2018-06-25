using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace RNCryptor
{
	public abstract class Cryptor
	{
		protected AesMode aesMode;

		protected Options options;

		protected bool hmac_includesHeader;

		protected bool hmac_includesPadding;

		protected HmacAlgorithm hmac_algorithm;

		protected const Algorithm algorithm = Algorithm.AES;

		protected const short saltLength = 8;

		protected const short ivLength = 16;

		protected const Pbkdf2Prf pbkdf2_prf = Pbkdf2Prf.SHA1;

		protected const int pbkdf2_iterations = 10000;

		protected const short pbkdf2_keyLength = 32;

		protected const short hmac_length = 32;

		public Encoding TextEncoding
		{
			get;
			set;
		}

		public Cryptor()
		{
			this.TextEncoding = Encoding.UTF8;
		}

		protected byte[] assembleHeader(PayloadComponents components)
		{
			List<byte> nums = new List<byte>();
			nums.AddRange(components.schema);
			nums.AddRange(components.options);
			nums.AddRange(components.salt);
			nums.AddRange(components.hmacSalt);
			nums.AddRange(components.iv);
			return nums.ToArray();
		}

        //private byte[] bitwiseXOR(byte[] first, byte[] second)
        //{
        //	byte[] numArray = new byte[(int)first.Length];
        //	ulong length = (ulong)((int)second.Length);
        //	ulong num = (ulong)((int)first.Length);
        //	ulong num1 = (ulong)0;
        //	for (ulong i = (ulong)0; i < num; i += (long)1)
        //	{
        //		//numArray[(void*)(checked((IntPtr)i))] = (byte)(first[(void*)(checked((IntPtr)i))] ^ second[(void*)(checked((IntPtr)num1))]);
        //       numArray[(void*)(checked((IntPtr)i))] = (byte)(first[(void*)(checked((IntPtr)i))] ^ second[(void*)(checked((IntPtr)num1))]);
        //        ulong num2 = num1 + (long)1;
        //		num1 = num2;
        //		num1 = (num2 < length ? num1 : (ulong)((long)0));
        //	}
        //	return numArray;
        //}
        private byte[] bitwiseXOR(byte[] first, byte[] second)
        {
            byte[] output = new byte[first.Length];
            ulong klen = (ulong)second.Length;
            ulong vlen = (ulong)first.Length;
            ulong k = 0;
            ulong v = 0;
            for (; v < vlen; v++)
            {
                output[v] = (byte)(first[v] ^ second[k]);
                k = (++k < klen ? k : 0);
            }
            return output;
        }

        private byte[] computeAesCtrLittleEndianCounter(int payloadLength, byte[] iv)
		{
			byte[] numArray = new byte[(int)iv.Length];
			iv.CopyTo(numArray, 0);
			int num = (int)Math.Ceiling((decimal)payloadLength / (decimal)iv.Length);
			List<byte> nums = new List<byte>();
			for (int i = 0; i < num; i++)
			{
				nums.AddRange(numArray);
				ref byte numPointer = ref numArray[0];
				numPointer = (byte)(numPointer + 1);
			}
			return nums.ToArray();
		}

		protected void configureSettings(Schema schemaVersion)
		{
			switch (schemaVersion)
			{
				case Schema.V0:
				{
					this.aesMode = AesMode.CTR;
					this.options = Options.V0;
					this.hmac_includesHeader = false;
					this.hmac_includesPadding = true;
					this.hmac_algorithm = HmacAlgorithm.SHA1;
					break;
				}
				case Schema.V1:
				{
					this.aesMode = AesMode.CBC;
					this.options = Options.V1;
					this.hmac_includesHeader = false;
					this.hmac_includesPadding = false;
					this.hmac_algorithm = HmacAlgorithm.SHA256;
					break;
				}
				case Schema.V2:
				case Schema.V3:
				{
					this.aesMode = AesMode.CBC;
					this.options = Options.V1;
					this.hmac_includesHeader = true;
					this.hmac_includesPadding = false;
					this.hmac_algorithm = HmacAlgorithm.SHA256;
					break;
				}
			}
		}

		protected byte[] encryptAesCtrLittleEndianNoPadding(byte[] plaintextBytes, byte[] key, byte[] iv)
		{
			byte[] numArray = this.computeAesCtrLittleEndianCounter((int)plaintextBytes.Length, iv);
			byte[] numArray1 = this.encryptAesEcbNoPadding(numArray, key);
			return this.bitwiseXOR(plaintextBytes, numArray1);
		}

		private byte[] encryptAesEcbNoPadding(byte[] plaintext, byte[] key)
		{
			byte[] array;
			Aes ae = Aes.Create();
			ae.Mode = CipherMode.ECB;
			ae.Padding = PaddingMode.None;
			ICryptoTransform cryptoTransform = ae.CreateEncryptor(key, null);
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

		protected byte[] generateHmac(PayloadComponents components, string password)
		{
			List<byte> nums = new List<byte>();
			if (this.hmac_includesHeader)
			{
				nums.AddRange(this.assembleHeader(components));
			}
			nums.AddRange(components.ciphertext);
			byte[] numArray = this.generateKey(components.hmacSalt, password);
			HMAC hMACSHA1 = null;
			HmacAlgorithm hmacAlgorithm = this.hmac_algorithm;
			if (hmacAlgorithm == HmacAlgorithm.SHA1)
			{
				hMACSHA1 = new HMACSHA1(numArray);
			}
			else if (hmacAlgorithm == HmacAlgorithm.SHA256)
			{
				hMACSHA1 = new HMACSHA256(numArray);
			}
			List<byte> nums1 = new List<byte>();
			nums1.AddRange(hMACSHA1.ComputeHash(nums.ToArray()));
			if (this.hmac_includesPadding)
			{
				for (int i = nums1.Count; i < 32; i++)
				{
					nums1.Add(0);
				}
			}
			return nums1.ToArray();
		}

		protected byte[] generateKey(byte[] salt, string password)
		{
			return (new Rfc2898DeriveBytes(password, salt, 10000)).GetBytes(32);
		}

		protected string hex_encode(byte[] input)
		{
			string str = "";
			byte[] numArray = input;
			for (int i = 0; i < (int)numArray.Length; i++)
			{
				byte num = numArray[i];
				str = string.Concat(str, string.Format("{0:x2}", num));
			}
			return str;
		}
	}
}