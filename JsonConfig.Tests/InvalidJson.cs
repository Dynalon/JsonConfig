using System;
using NUnit.Framework;

namespace JsonConfig.Tests
{
	[TestFixture()]
	public class InvalidJson
	{
		[Test]
		[ExpectedException (typeof(JsonFx.Serialization.DeserializationException))]
		public void EvidentlyInvalidJson ()
		{
			var c = new Config ();
			c.ApplyJson ("jibberisch");
		}
		[Test]
		[ExpectedException (typeof(JsonFx.Serialization.DeserializationException))]
		public void MissingObjectIdentifier()
		{	
			var c = new Config ();
			var invalid_json = @" { [1, 2, 3] }";	
			c.ApplyJson (invalid_json);
		}
	}
}

