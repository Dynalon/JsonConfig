using System;
using NUnit.Framework;
using JsonFx.Json;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using JsonConfig;

namespace JsonConfig.Tests
{
	[TestFixture]
	public abstract class BaseTest
	{
		public static dynamic GetUUT(string name)
		{
			// read in all our JSON objects
			var jsonTests = Assembly.GetExecutingAssembly ().GetManifestResourceStream ("JsonConfig.Tests.JSON." + name + ".json");
			var sReader = new StreamReader (jsonTests);
			return Config.ApplyJson (sReader.ReadToEnd (), new ConfigObject ());
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

