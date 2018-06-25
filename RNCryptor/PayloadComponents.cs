using System;

namespace RNCryptor
{
	public struct PayloadComponents
	{
		public byte[] schema;

		public byte[] options;

		public byte[] salt;

		public byte[] hmacSalt;

		public byte[] iv;

		public int headerLength;

		public byte[] hmac;

		public byte[] ciphertext;
	}
}