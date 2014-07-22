using System;
using System.IO;
using NUnit.Framework;

namespace JsonConfig.Tests
{
	[TestFixture()]
	public class InvalidJson
	{
		[Test]
        [ExpectedException(typeof(InvalidDataException))]
		public void EvidentlyInvalidJson ()
		{
			dynamic scope = Config.Global;
			scope.ApplyJson ("jibberisch");
		}
		[Test]
        [ExpectedException(typeof(InvalidDataException))]
		public void MissingObjectIdentifier()
		{	
			dynamic scope = Config.Global;
			var invalid_json = @" { [1, 2, 3] }";	
			scope.ApplyJson (invalid_json);
		}
	}
}

