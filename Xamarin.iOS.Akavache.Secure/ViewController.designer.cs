// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Xamarin.iOS.Akavache.Secure
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UISegmentedControl cacheType { get; set; }

		[Action ("button1Action:")]
		partial void button1Action (Foundation.NSObject sender);

		[Action ("button2Action:")]
		partial void button2Action (Foundation.NSObject sender);

		[Action ("button3Action:")]
		partial void button3Action (Foundation.NSObject sender);

		[Action ("button4Action:")]
		partial void button4Action (Foundation.NSObject sender);

		[Action ("button5Action:")]
		partial void button5Action (Foundation.NSObject sender);

		[Action ("button6Action:")]
		partial void button6Action (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (cacheType != null) {
				cacheType.Dispose ();
				cacheType = null;
			}
		}
	}
}
