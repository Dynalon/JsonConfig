using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Microsoft.CSharp;

using JsonFx;
using NUnit.Framework;
using JsonConfig;


namespace JsonConfig.Tests
{
	[TestFixture]
	public class BunchOfTests : BaseTest
	{
		[Test]
		public void Product ()
		{
			dynamic parsed = GetUUT ("Product");	
			dynamic merged = Merger.Merge (parsed.Amazon, parsed.WalMart);
	
			Assert.That (merged.Price == 129);
			Assert.That (merged.Rating.Comments.Length == 3);
			
			// only float values should be in the rating
			var stars = merged.Rating.Stars as ICollection<double>;
			Assert.IsNotNull (stars);
			
			Assert.That (stars.Sum (d => d) == 12.5);
		}
		[Test]
		public void Arrays ()
		{
			dynamic parsed = GetUUT("Arrays");
			dynamic merged = Merger.Merge (parsed.Fruit1, parsed.Fruit2);
		
			var fruitList = merged.Fruit as ICollection<string>;
			Assert.AreEqual (6, fruitList.Count);
			// apple must be in it 2 times, since array merging is NOT SET merging!
			Assert.AreEqual (fruitList.Count (f => f == "apple"), 2);
			Assert.That (fruitList.Contains ("coconut"));
		}
		[Test]
		public void FirewallConfig ()
		{
			dynamic parsed = GetUUT ("Firewall");
			dynamic merged = Merger.Merge (parsed.UserConfig, parsed.FactoryDefault);
	
			var interfaces = merged.Interfaces as ICollection<string>;
			Assert.AreEqual (3, interfaces.Count ());
		
			var zones = merged.Zones as ICollection<dynamic>;
			
			var loopback = zones.Count (d => d.Name == "Loopback");
			Assert.AreEqual (1, loopback);
			
			// one portmapping is present at least
			var intzone = zones.Where (d => d.Name == "Internal").First ();
			Assert.That (intzone.PortMapping != null);
			Assert.Greater (intzone.PortMapping.Length, 0);
		}
		[Test]
		public void DefaultConfigFound ()
		{
			var c = new Config ();
			Assert.IsNotNull (c.DefaultConfig);
			Assert.That (c.DefaultConfig.Default == "found");
		}
	}
}