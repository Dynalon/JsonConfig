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
			dynamic scope = Config.Scope;
			scope.ApplyJson ("jibberisch");
		}
		[Test]
		[ExpectedException (typeof(JsonFx.Serialization.DeserializationException))]
		public void MissingObjectIdentifier()
		{	
			dynamic scope = Config.Scope;
			var invalid_json = @" { [1, 2, 3] }";	
			scope.ApplyJson (invalid_json);
		}
	}
}

