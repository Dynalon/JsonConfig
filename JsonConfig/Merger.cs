using System.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace JsonConfig
{
	public static class Merger
	{
		/// <summary>
		/// Merge the specified obj1 and obj2, where obj1 has precendence.
		/// </summary>
		/// <param name='obj1'>
		/// Obj1. If null, result will be null.
		/// </param>
		/// <param name='obj2'>
		/// Obj2. If null, result will be Obj1
		/// </param>
		/// <exception cref='TypeMissmatchException'>
		/// Is thrown when the type missmatch exception.
		/// </exception>
		public static dynamic Merge (dynamic obj1, dynamic obj2)
		{
			// handle what happens if one of the args is null
			if (obj1 == null) return null;
			if (obj2 == null) return obj1;
		
			if (obj1.GetType () != obj2.GetType ())	
				throw new TypeMissmatchException ();
			
			// ExpandoObject implements dictionary
			// and changes in the dictionary WILL REFLECT back to the
			var dict1 = obj1 as IDictionary<string, object>;
			var dict2 = obj2 as IDictionary<string, object>;

			dynamic result = new ExpandoObject ();
			var rdict = result as IDictionary<string, object>;
			
			// first, copy all non colliding keys over
	        foreach (var kvp in dict1)
				if (!dict2.Keys.Contains (kvp.Key))
					rdict.Add (kvp);
	        foreach (var kvp in dict2)
				if (!dict1.Keys.Contains (kvp.Key))
					rdict.Add (kvp);
		
			// now handle the colliding keys	
			foreach (var kvp1 in dict1) {
				// skip already copied over keys
				if (!dict2.Keys.Contains (kvp1.Key) || dict2[kvp1.Key] == null)
					continue;
	
				var kvp2 = new KeyValuePair<string, object> (kvp1.Key, dict2[kvp1.Key]);
				
				// some shortcut variables to make code more readable		
				var key = kvp1.Key;
				var value1 = kvp1.Value;
				var value2 = kvp2.Value;
				var type1 = value1.GetType ();
				var type2 = value1.GetType ();
				
				// check if both are same type
				if (type1 != type2)
					throw new TypeMissmatchException ();

				if (value1 is ExpandoObject[]) {
					rdict[key] = CollectionMerge (value1, value2);
					/*var d1 = val1 as IDictionary<string, object>;
					var d2 = val2 as IDictionary<string, object>;
					rdict[key] = CollectionMerge (val1, val2); */
				}
				else if (value1 is ExpandoObject) {
					rdict[key] = Merge ((ExpandoObject) value1, (ExpandoObject) value2);
				}
				else if (value1 is string)
				{
					rdict[key]	= value1;
				}
				else if (value1 is IEnumerable) {
					rdict[key] = CollectionMerge (value1, value2);
				}
				else {
					rdict[key] = value1;
				}					
				
				//else if (kvp.Value.GetType ().IsByRef) {
					// recursively merge it	
				//}
			}
			return result;
		}
		public static dynamic CollectionMerge (dynamic obj1, dynamic obj2)
		{
			var x = new ArrayList ();
			x.AddRange (obj1);
			x.AddRange (obj2);
			return x.ToArray (obj1.GetType ().GetElementType ());
		}
	}
	/// <summary>
	/// Get thrown if two types do not match and can't be merges
	/// </summary>	
	public class TypeMissmatchException : Exception
	{
	}
}
