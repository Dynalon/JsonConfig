using System;
using System.Linq;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.IO;

using JsonFx;
using JsonFx.Json;
using System.Text.RegularExpressions;

namespace JsonConfig 
{
	public class Config {
		public dynamic DefaultConfig = null;
		public dynamic UserConfig = null;
	
		///<summary>
		///	scope config will represent the current, actual config
		/// after merging/inheriting from Default & UserConfig
		/// </summary>
		public dynamic ScopeConfig = null; 
		
		public Config ()
		{
			var assembly = System.Reflection.Assembly.GetCallingAssembly ();
			DefaultConfig = getDefaultConfig (assembly);
			
			// default config gets override by user defined config
			var executionPath = AppDomain.CurrentDomain.BaseDirectory;
			var userConfigFile = Path.Combine (executionPath, "settings.conf");
			if (File.Exists (userConfigFile)) {
				UserConfig = ParseJson (File.ReadAllText (userConfigFile));
			}
			ScopeConfig = Merger.Merge (UserConfig, DefaultConfig);
			
		}
		public dynamic ApplyJsonFromFile (string overlayConfigPath) 
		{
			var overlay_json = File.ReadAllText (overlayConfigPath);
			dynamic overlay_config = ParseJson (overlay_json);
	
			return Merger.Merge (overlay_config, ScopeConfig);	
		}
		public dynamic ApplyJson (string jsonConfig)
		{
			dynamic jsonconfig = ParseJson (jsonConfig);
			return Merger.Merge (jsonconfig, DefaultConfig);
		}
		public static dynamic ParseJson (string json)
		{
			var lines = json.Split (new char[] {'\n'});
			// remove lines that start with a dash # character 
			var filtered = from l in lines
				where !(Regex.IsMatch (l, @"^\s*#(.*)"))
				select l;
			
			var filtered_json = string.Join ("\n", filtered);
			
			var json_reader = new JsonReader ();
			dynamic parsed = json_reader.Read (filtered_json);
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
			
			if(string.IsNullOrEmpty (dconf_resource))
				return null;
		
			var stream = assembly.GetManifestResourceStream (dconf_resource);
			string default_json = new StreamReader(stream).ReadToEnd ();
			return default_json;
		}
		public bool ScopeMemberExists (string name)
		{
			return MemberExists (ScopeConfig, name);
		}
		// TODO have this as Enumerator/Indexer MemberExists(
		public static bool MemberExists (ExpandoObject d, string name)
		{
			var dict = d as IDictionary<string, object>;
			if (dict.ContainsKey (name))
				return true;
			return false;
		}
		
	}
}
