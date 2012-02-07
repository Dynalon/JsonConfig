using System;
using System.Linq;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.IO;

using JsonFx;
using JsonFx.Json;

namespace JsonConfig 
{
	public class Config {
		public dynamic DefaultConfig = null;
		public Config ()
		{
			var assembly = System.Reflection.Assembly.GetCallingAssembly ();
			DefaultConfig = getDefaultConfig (assembly);
		}
		public dynamic ApplyFile (string userConfigPath) 
		{
			var userconfig_json = File.ReadAllText (userConfigPath);
			dynamic userconfig = ParseJson (userconfig_json);
	
			return Merger.Merge (userconfig, DefaultConfig);	
		}
		public dynamic ApplyJson (string jsonConfig)
		{
			dynamic userconfig = ParseJson (jsonConfig);
			return Merger.Merge (userconfig, DefaultConfig);
		}
		public static dynamic ParseJson (string json)
		{
			var json_reader = new JsonReader ();
			dynamic parsed = json_reader.Read (json);
			return parsed;
		}
		protected dynamic getDefaultConfig (Assembly assembly)
		{
			var dconf_json = scanForDefaultConfig (assembly);
			if (dconf_json == null)
				return null;
			return ParseJson (dconf_json);
		}
		protected string scanForDefaultConfig(Assembly assembly)
		{
			if(assembly == null)
				assembly = System.Reflection.Assembly.GetEntryAssembly ();
			
			string[] res = assembly.GetManifestResourceNames ();
			
			var dconf_resource = res.Where (r =>
					r.EndsWith ("default.conf", StringComparison.OrdinalIgnoreCase) ||
					r.EndsWith ("default.conf.json", StringComparison.OrdinalIgnoreCase))
				.FirstOrDefault ();
			
		
			//foreach(string s in res)
				//Console.WriteLine ("res {0}", s);
			if(string.IsNullOrEmpty (dconf_resource))
				return null;
		
			var stream = assembly.GetManifestResourceStream (dconf_resource);
			string default_json = new StreamReader(stream).ReadToEnd ();
			return default_json;
			
		}
	}
}
