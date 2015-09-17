using System;
using NUnit.Framework;

using JsonConfig;
using System.Dynamic;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JsonConfig.Tests
{
	[TestFixture()]
	public class TypeTests : BaseTest
	{
		[Test()]
		public void NestedExpandoConvertToConfigObject()
		{
			dynamic e = new ExpandoObject ();
			e.Foo = "bar";
			e.X = 1;
			dynamic f = new ExpandoObject ();
			f.Foo = "bar";
			f.X = 1;

			e.Nested = f;

			dynamic c = ConfigObject.FromExpando (e);

			Assert.IsInstanceOfType (typeof (ConfigObject), c);
			Assert.IsInstanceOfType (typeof (ConfigObject), c.Nested);
			Assert.AreEqual ("bar", c.Foo);
			Assert.AreEqual (1, c.X);

			Assert.AreEqual ("bar", c.Nested.Foo);
			Assert.AreEqual (1, c.Nested.X);
		}
		[Test]
		public void DeeplyNestedExpandoConvert ()
		{
			// can't use GetUUT here since this will already involve conversion
			var name = "Types";
			var jsonTests = Assembly.GetExecutingAssembly ().GetManifestResourceStream ("JsonConfig.Tests.JSON." + name + ".json");
			var sReader = new StreamReader (jsonTests);
			dynamic parsed = JsonConvert.DeserializeObject<ExpandoObject>(sReader.ReadToEnd (), new ExpandoObjectConverter());

			dynamic config = ConfigObject.FromExpando (JsonNetAdapter.Transform(parsed));

			Assert.AreEqual ("bar", config.Foo);
			Assert.AreEqual ("bar", ((ICollection<dynamic>) config.NestedArray).First ().Foo);
			Assert.AreEqual ("bar", config.DoubleNestedArray[0].One[0].Foo);

			Assert.IsInstanceOfType (typeof (ConfigObject[]), config.DoubleNestedArray[0].One);
			Assert.AreEqual ("bar", config.DoubleNestedArray[0].One[0].Foo);
			Assert.AreEqual (4, config.DoubleNestedArray[0].One.Length);

			Assert.AreEqual ("bar", config.DoubleNestedArray[1].Two[0].Foo);
			Assert.AreEqual ("bar", config.DoubleNestedArray[1].Two[3].Foo);
			Assert.AreEqual ("bar", config.DoubleNestedArray[1].Two[3].Foo);
		}
		[Test]
		public void SimpleExpandoToConfigObject ()
		{
			dynamic e = new ExpandoObject ();

			e.Foo = "bar";
			e.X = 1;

			var c = ConfigObject.FromExpando (e);

			Assert.IsInstanceOfType (typeof(ConfigObject), c);

			Assert.IsInstanceOfType (typeof(string), c.Foo);
			Assert.AreEqual ("bar", c.Foo);

			Assert.IsInstanceOfType (typeof(int), c.X);
			Assert.AreEqual (1, c.X);
		}
		[Test]
		public void CastNonExistantFields ()
		{
			int x = Config.Global.NonExistant;
			Assert.AreEqual (0, x);

			int[] xarray = Config.Global.NonExistant;
			Assert.AreEqual (0, xarray.Length);

			string[] sarray = Config.Global.NonExistant;
			Assert.AreEqual (0, sarray.Length);

			bool b = Config.Global.NonExistant;
			Assert.AreEqual (false, b);

			bool? bn = Config.Global.NonExistant;
			Assert.AreEqual (null, bn);
		}
		[Test]
		public void CastConfigObjectToBool ()
		{
			// if a ConfigObject has nested members, we wan't to be able to do
			// a fast check for non null in if statements:
			//
			// if (config.SomeMember) { ... }

			string conf = @"{ SomeMember: { Foo: 42 } }";
			dynamic c = new ConfigObject();
			c.ApplyJson(conf);

			bool t = (bool) c.SomeMember;
			Assert.AreEqual(true, t);

			bool f = (bool) c.NonExistantMember;
			Assert.AreEqual (f, false);
		}
	}
}

