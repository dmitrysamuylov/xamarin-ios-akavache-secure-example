using System.Reactive.Concurrency;
using Akavache;
using Akavache.Sqlite3;

namespace Xamarin.iOS.Akavache.Secure
{
    public class ApiService
    {
        static ApiService instance = new ApiService();

        // Api Cache
        IBlobCache cache = null;
        public IBlobCache ApiCache
        {
            get
            {
                return cache;
            }
        }

		// Api Encryption Provider
		CustomEncryptionProvider crypto;
		public CustomEncryptionProvider ApiCrypto
		{
			get
			{
				return crypto;
			}
		}

        protected ApiService()
        {
            crypto = new CustomEncryptionProvider();			
            cache = new SQLiteEncryptedBlobCache("secure.db", crypto, TaskPoolScheduler.Default);
        }

        public static ApiService DefaultService
        {
            get
            {
                return instance;
            }
        }
    }
}