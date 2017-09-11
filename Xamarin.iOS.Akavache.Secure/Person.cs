using System;
using System.Collections.Generic;

namespace Xamarin.iOS.Akavache.Secure
{
	public class Person
	{
		public string Name { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
		public double Number1 { get; set; }
		public double Number2 { get; set; }
		public double Number3 { get; set; }
		public double Number4 { get; set; }
        public List<string> NamesOfFriends { get; set; }
		public DateTimeOffset LastUpdateTime { get; set; }
	}
}
