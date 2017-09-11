/*
Taken from http://kent-boogaart.com/blog/password-protected-encryption-provider-for-akavache
*/
using System;
using System.IO;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Security.Cryptography;
using System.Text;
using Akavache;

namespace Xamarin.iOS.Akavache.Secure
{
	public interface IPasswordProtectedEncryptionProvider : IEncryptionProvider
	{
		void SetPassword(string password);
	}

    public sealed class CustomEncryptionProvider : IPasswordProtectedEncryptionProvider
	{
        static readonly byte[] salt = Encoding.ASCII.GetBytes("dVBZMQWyFRcJOIas");
        readonly IScheduler scheduler;
		readonly SymmetricAlgorithm symmetricAlgorithm;
		ICryptoTransform decryptTransform;
		ICryptoTransform encryptTransform;

		public CustomEncryptionProvider()
		{
            scheduler = BlobCache.TaskpoolScheduler ?? throw new ArgumentNullException(nameof(scheduler), "Scheduler instance is null");
			symmetricAlgorithm = new RijndaelManaged();
		}

        public void SetPassword()
        {
            // this could be set from a programmatically generated password that is kept in keychain on iOS for example
			var securePassword = "kadhaskdhsakhaskjdhaskjdhaskdjashdkjahkfghkjhew";
			SetPassword(securePassword);
        }

		public void SetPassword(string password)
		{
            if(password == null)
               throw new ArgumentNullException(nameof(password), "password can't be null");

			var derived = new Rfc2898DeriveBytes(password, salt);
			var bytesForKey = symmetricAlgorithm.KeySize / 8;
			var bytesForIV = symmetricAlgorithm.BlockSize / 8;
			symmetricAlgorithm.Key = derived.GetBytes(bytesForKey);
			symmetricAlgorithm.IV = derived.GetBytes(bytesForIV);
			decryptTransform = symmetricAlgorithm.CreateDecryptor(symmetricAlgorithm.Key, symmetricAlgorithm.IV);
			encryptTransform = symmetricAlgorithm.CreateEncryptor(symmetricAlgorithm.Key, symmetricAlgorithm.IV);
		}

		public IObservable<byte[]> DecryptBlock(byte[] block)
		{
            if (block == null)
            {
                throw new ArgumentNullException(nameof(block), "block can't be null");
            }

			if (decryptTransform == null)
			{
				return Observable.Throw<byte[]>(new InvalidOperationException("You must call SetPassword first."));
			}

			return Observable.Start(() => InMemoryTransform(block, decryptTransform), scheduler);
		}

		public IObservable<byte[]> EncryptBlock(byte[] block)
		{
            if (block == null)
            {
                throw new ArgumentNullException(nameof(block), "block can't be null");
            }

			if (encryptTransform == null)
			{
				return Observable.Throw<byte[]>(new InvalidOperationException("You must call SetPassword first."));
			}

			return Observable.Start(() => InMemoryTransform(block, encryptTransform), scheduler);
		}

		static byte[] InMemoryTransform(byte[] bytesToTransform, ICryptoTransform transform)
		{
			using (var memoryStream = new MemoryStream())
			{
				using (var cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
				{
					cryptoStream.Write(bytesToTransform, 0, bytesToTransform.Length);
				}

				return memoryStream.ToArray();
			}
		}
	}
}
