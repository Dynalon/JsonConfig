using System;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace JsonConfig.Tests
{
	[TestFixture()]
	public class ConfigFromDirectory
	{
		private string configFolder ()
		{
			return Directory.GetCurrentDirectory () + "/../../DirectoryMerge/";
		}
		[Test()]
		public void AllArraysFoundAndMerged()
		{
			dynamic config = Config.ApplyFromDirectory (configFolder (), null, true);
			Assert.That (config.Apples is string[]);
			List<string> apples = ((string[]) config.Apples).ToList<string> ();
			Assert.Contains ("Golden Delicious", apples);
			Assert.AreEqual (apples.Count, 9);

			// no element is included twice
			Assert.AreEqual (apples.Count(), apples.Distinct ().Count ());

		}
		[Test()]
		public void AllFilesWereFound()
		{
			dynamic config = Config.ApplyFromDirectory (configFolder (), null, true);
			Assert.AreEqual (true, config.Fruits);
			Assert.AreEqual (true, config.MoreFruits);
			Assert.AreEqual (true, config.EvenMoreFruits);
			Assert.AreEqual (true, config.EvenEvenMoreFruits);
		}
	}
}

