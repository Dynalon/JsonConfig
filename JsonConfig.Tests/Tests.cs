using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Microsoft.CSharp;

using JsonFx;
using NUnit.Framework;
using JsonConfig;
using System.Reflection;


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
		public void CanAccessNonExistantField ()
		{
			dynamic parsed = GetUUT("Arrays");
			dynamic merged = Merger.Merge (parsed.Fruit1, parsed.Fruit2);

			Assert.That (merged.field.not.exist.ToString () == null);
			Assert.That (string.IsNullOrEmpty (merged.thisfield.does.just.not.exist) == true);

		}
		[Test]
		public void ArrayWithEmptyArray ()
		{
			dynamic parsed = GetUUT("Arrays");
			dynamic merged = Merger.Merge (parsed.Fruit1, parsed.EmptyFruit);

			var fruitList = merged.Fruit as ICollection<string>;
			Assert.AreEqual (3, fruitList.Count);
			Assert.That (fruitList.Contains ("apple"));
			Assert.That (fruitList.Contains ("banana"));
			Assert.That (fruitList.Contains ("melon"));
		}
		[Test]
		public void ComplexArrayWithEmptyArray ()
		{
			dynamic parsed = GetUUT("Arrays");
			dynamic merged = Merger.Merge (parsed.Coords1, parsed.Coords2);
			
			Assert.AreEqual (2, merged.Pairs.Length);
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
			Assert.IsNotNull (Config.Default);
			Assert.That (Config.Default.Sample == "found");
		}
		[Test]
		public void ComplexTypeWithArray ()
		{
			dynamic parsed = GetUUT ("Foods");
			dynamic fruit = parsed.Fruits;
			dynamic vegetables = parsed.Vegetables;

			dynamic result = Merger.Merge (fruit, vegetables);

			Assert.AreEqual (6, result.Types.Length);
			Assert.IsInstanceOfType (typeof(ConfigObject), result);
			Assert.IsInstanceOfType (typeof(ConfigObject[]), result.Types);
		}
		[Test]
		public void ManualDefaultAndUserConfig ()
		{
			dynamic parsed = GetUUT ("Foods");

			Config.SetUserConfig (parsed.Fruits);
			Config.SetDefaultConfig (parsed.Vegetables);

			Assert.IsInstanceOfType (typeof(ConfigObject), Config.User);
			Assert.IsInstanceOfType (typeof(ConfigObject), Config.Default);

			dynamic scope = Config.Scope;
			scope = scope.ApplyJson (@"{ Types : [{Type : ""Salad"", PricePerTen : 5 }]}");
			Assert.AreEqual (7, scope.Types.Length);
		}
		[Test]
		public void EnabledModulesTest ()
		{
			// classical module scenario: user specifies what modules are to be loaded

			dynamic modules = GetUUT ("EnabledModules");

			// method one : use an object with each module name as key, and value true/false
			dynamic modules_object = modules.EnabledModulesObject;
			Assert.AreNotEqual (null, modules_object.Module1);
			Assert.AreNotEqual (null, modules_object.Module2);
			Assert.That (modules_object.Module2 == false);

			// tricky part: NonExistantModule is not defined in the json but should be false anyways
			Assert.That (modules_object.NonExistantModule == false);
			Assert.That (modules_object.NonExistantModule.Nested.Field.That.Doesnt.Exist == false);
		}
	}
}