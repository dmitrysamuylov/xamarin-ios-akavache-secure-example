/*
Parts taken from https://github.com/PureWeen/Akavache.Samples
*/
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Akavache;
using Foundation;
using UIKit;
using System.Reactive.Linq;

namespace Xamarin.iOS.Akavache.Secure
{
    public partial class ViewController : UIViewController
    {
        public enum CacheType { Local, Secure }
        readonly string personKey = "Person";
        readonly string jsonKey = "JsonData";

        CacheType StoreType
        {
            get 
            {
                var selectedSegment = cacheType.TitleAt(cacheType.SelectedSegment);
                return selectedSegment == "Secure" ? CacheType.Secure : CacheType.Local;
            }
        }

		ExplorerType CacheExplorerType
		{
			get
			{
				var selectedSegment = cacheType.TitleAt(cacheType.SelectedSegment);
                return StoreType == CacheType.Secure ? ExplorerType.Secure : ExplorerType.Local;
			}
		}

        IBlobCache CurrentBlobCache
		{
			get
			{
                return StoreType == CacheType.Secure ? ApiService.DefaultService.ApiCache : BlobCache.LocalMachine;
			}
		}

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        // cache Person object
		partial void button1Action(NSObject sender)
        {
            var person = new Person
            {
                Name = "John Smith",
                DateOfBirth = DateTime.Parse("1/1/1960"),
                LastUpdateTime = DateTime.Now,
                Number1 = 12321,
                Number2 = 12321.545423,
                Number3 = 21.56755756756756,
                Number4 = -0.13123,
                NamesOfFriends = new List<string>{ "John", "Jane", "Joe" }
            };

            // cache for 1 day
            CurrentBlobCache.InsertObject(personKey, person, DateTime.Now.AddDays(1));
        }

        // view Person object in cache
		partial void button2Action(NSObject sender)
        {
            PresentViewController(Explorer.GetNavigationController(CacheExplorerType), true, null);
        }

        // delete Person object from cache
		partial void button3Action(NSObject sender)
        {
            CurrentBlobCache.InvalidateObject<Person>(personKey);
        }

        // cache JSON from a URL
		partial void button4Action(NSObject sender)
        {
            ApiService.DefaultService.ApiCrypto.SetPassword();
            ApiService.DefaultService.ApiCache.GetOrFetchObject(jsonKey, async () => {
				try
				{
					var url = "https://jsonplaceholder.typicode.com/posts";
					var httpClient = new HttpClient();
					var response = await httpClient.GetAsync(url);
					var result = "N/A";

					if (response.IsSuccessStatusCode)
					{
						result = await response.Content.ReadAsStringAsync();
						return result;
					}
				}
				catch (Exception ex)
				{
					Console.Error.WriteLine(@"ERROR - GetJsonTest, error: {0}", ex.Message);
				}

				return "(null)";
			}, DateTime.Now.AddDays(1))
			.Do(_ => InvokeOnMainThread(() =>
			{
				// UI updates if needed
			}))
			.Catch((Exception exc) =>
			{
				//InvokeOnMainThread(() => );
				return Observable.Empty<string>();
			})
			.Subscribe();
		}

        // view JSON from a URL in cache
		partial void button5Action(NSObject sender)
        {
            PresentViewController(Explorer.GetNavigationController(CacheExplorerType), true, null);
        }

        // delete JSON from a URL in the cache
		partial void button6Action(NSObject sender)
        {
            CurrentBlobCache.InvalidateObject<string>(jsonKey);
        }
	}
}
