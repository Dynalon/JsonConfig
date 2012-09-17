using System;
using System.Collections.Generic;
using System.Dynamic;
using JsonFx.Json;

using JsonConfig;

namespace JsonConfig.Tests
{

/* kept for debugging reasons */
	public static class MainClass
	{
		public static void Main (string[] args)
		{
//			dynamic s = (DynamicObject) new ConfigObject ();
//			dynamic t = (DynamicObject) new ConfigObject ();
//			s.Foo = 7;
//			t.Foo = 5;
//
//			IDictionary<string,object> dt = t.members;
//			IDictionary<string,object> ds = s.members;
//
//			IValueInjecter inj = new ValueInjecter ();
//
//			Dictionary<string, object> sd = new Dictionary<string, object> ();
//			Dictionary<string, object> td = new Dictionary<string, object> ();
//			sd.Add ("foobar", 3);
//			td.Add ("foobar" ,5);
//			var x = inj.Inject (sd, td);
//
//			Console.WriteLine (x);

		}
		public static void PrintDictionary (IDictionary<string, object> dictionary, uint spacecount = 0)
		{
			foreach (var kvp in dictionary) {
				var val = kvp.Value;
				var type = val.GetType ();
				if (type == typeof(ExpandoObject[])) {
					foreach (var array_elem in (ExpandoObject[]) val) {
						PrintDictionary (array_elem, spacecount + 1);
					}
				}
				var new_kvp = kvp.Value as IDictionary<string, object>;
				if (new_kvp != null) {
					Console.WriteLine(kvp.Key);
					PrintDictionary (new_kvp, spacecount + 1);
				}
				else {
					for (uint i = spacecount; i > 0; i--)
						Console.Write("\t");
					Console.WriteLine ("{1} [{0}]: {2}", kvp.Value.GetType (), kvp.Key, kvp.Value);
				}
			}
		}
	}
}