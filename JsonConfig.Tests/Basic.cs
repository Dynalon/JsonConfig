using System;
using NUnit.Framework;
using JsonFx.Json;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace JsonConfig.Tests
{
	[TestFixture]
	public abstract class BaseTest
	{
		public static dynamic GetUUT(string name)
		{
			// read in all our JSON objects
			var reader = new JsonReader ();
			var jsonTests = Assembly.GetExecutingAssembly ().GetManifestResourceStream ("JsonConfig.Tests.JSON." + name + ".json");
			var sReader = new StreamReader (jsonTests);	
			return reader.Read (sReader.ReadToEnd ());
		}
		
		[SetUp]
		public void SetUp ()
		{
		}
		[TearDown]
		public void TearDown ()
		{
		}
	}
	
}

