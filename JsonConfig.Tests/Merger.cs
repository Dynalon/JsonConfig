using System;
using NUnit.Framework;
using System.Dynamic;

namespace JsonConfig.Tests
{
	[TestFixture()]
	public class MergerTest
	{
		[Test]
		public void FirstObjectIsNull()
		{
			dynamic x = 1;
			dynamic result = JsonConfig.Merger.Merge (null, x);
			
			Assert.That (result == null);
		}
		[Test]
		public void SecondObjectIsNull ()
		{
			dynamic x = 1;
			dynamic result = JsonConfig.Merger.Merge (x, null);
			Assert.That (result == 1);
		}
		[Test]
		public void BothObjectsAreNull ()
		{
			dynamic result = JsonConfig.Merger.Merge (null, null);
			
			Assert.IsNull (result);
		}
		[Test]
		[ExpectedException(typeof(JsonConfig.TypeMissmatchException))]
		public void TypesAreDifferent ()
		{
			dynamic x = "somestring";
			dynamic y = 1;
			dynamic result = JsonConfig.Merger.Merge (x, y);
		}
	}
}

