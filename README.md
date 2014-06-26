JsonConfig README
=====================

## About
JsonConfig is a simple to use configuration library, allowing JSON based config
files for your C#/.NET application instead of cumbersome
web.config/application.config xml files.

It is based on JsonFX and C# 4.0 dynamic feature. Allows putting your programs
config file into .json files, where a default config can be embedded as a
resource or put in the (web-)application folder. Configuration can be accessed
via dynamic types, no custom classes or any other stub code is necessary.

JsonConfig brings support for *config inheritance*, meaning a set of
configuration files can be used to have a single, scoped configuration at
runtime which is a merged version of all provided configuration files.

## Example

Since my lack of skills in writing good examples into a documentation file, it
is best to take a look at the examples/ folder with a complete commented .sln
which will give you a better understanding (TODO).

### Getting started

Usually the developer wants a default configuration that is used when no
configuration by the user is present whatsoever. Often, this configuration is
just hardcoded default values within the code. With JsonConfig there is no need
for hardcoding, we simply create a default.conf file and embedd it as a
resource.

Let's create a sample default.conf for a hypothetical grocery store:

	# Lines beginning with # are skipped when the JSON is parsed, so we can
	# put comments into our JSON configuration files
	{
		StoreOwner : "John Doe",
		
		# List of items that we sell
		Fruits: [ "apple", "banana", "pear" ]
	}

JsonConfig automatically scan's all assemblies for the presence of a
default.conf file, so we do not have to add any boilerplate code and can
directly dive in:

	// exmaple code using our configuration file
	using JsonConfig;
	[...]
	public void PrintInfo () {
		var storeOwner = Config.Default.StoreOwner;

		Console.WriteLine ("Hi there, my name is {0}!", storeOwner);

		foreach (var fruit in Config.Default.Fruits)
			Console.WriteLine (fruit);

	}

However, the developer wants the user to make his own configuration file.
JsonConfig automatically scans for a settings.conf file in the root path of the
application.

	# sample settings.conf
	{
		Fruits: [ "melon", "peach" ]	
	}

The settings.conf and the default.conf are then merged in a clever
way and provided via the *Global* configuration.
```csharp
public void PrintInfo () {
	// will result in apple, banana, pear 
	foreach (var fruit in Config.Default.Fruits)
		Console.WriteLine (fruit);

	// will result in melon, peach
	foreach (var fruit in Config.User.Fruits)
		Console.WriteLine (fruit);

	// access the Global scope, which is a merge of Default
	// and User configuration
	// will result in apple, banana, pear, melon, peach
	foreach (var fruit in Config.Global.Fruits)
		Console.WriteLine (fruit);

}
```
### Nesting objects

We are not bound to any hierarchies, any valid JSON is a valid configuration
object. Take for example a hypothetical webserver configuration:

	{
		ListenPorts: [ 80, 443 ],
		EnableCaching : true,
		ServerProgramName: "Hypothetical WebServer 1.0",

		Websites: [
			{
				Path: "/srv/www/example/",
				Domain: "example.com",
				Contact: "admin@example.com"	
			},
			{
				Path: "/srv/www/somedomain/",
				Domain: "somedomain.com",
				Contact: "admin@somedomain.com"
			}
		]
	}	

Above configuration could be accessed via:

	using JsonConfig;
	[...]

	public void StartWebserver () {
		// access via Config.Global
		string serverName = Config.Global.ServerProgramName;
		bool caching = Config.Global.EnableCaching;
		int[] listenPorts = Config.Global.ListenPorts;

		foreach (dynamic website in Config.Global.Websites) {
			StartNewVhost (website.Path, website.Domain, website.Contact);
		}
	}

### "Magic" prevention of null pointer exceptions

Choosing reasonable default values is only a matter of supplying a good
default.conf. But using some C# 4.0 dynamic "magic", non-existant configuration
values will not throw a NullPointer exception:

	// we are lazy and do not want to give default values for configuration
	// objects, but just want them to be false

	// there is no need to have LoadedModules OR HttpServer in your
	// default.conf, if missing this will just evaluate to false
	if (Config.Global.LoadedModules.HttpServer) {
		// start HttpServer
	}

	// more drastic example, its safe to write
	if (Config.Global.nonexistant.field.that.never.will.be.given) {
		// this will never be run unless you create that structure in your
		// config files
	}

	// when the configuration value is cast to string, it will be null if not
	// given
	if (string.IsNullOrEmpty (Config.Global.some.nonexistant.nested.field)) {
		// will most likely be run all the times
	}

The "magic" allows you to cast a not-yet existing field to common types, which will then have empty or default values:

	foreach (string name in Config.Global.NonExistantField as string[]) {
		// instead of being cast to null, if a non-existing field is cast to string[] it
		// will just be an empty array: string[] { }
		Console.WriteLine (name);
	}

	// works for nullable types, too. Nullable types will
	// cast to null if not exsisting in the config.
	var processFiles = (bool?) Config.Global.ProcessFiles;
	if (processFiles != null) {
		// will only be run if ProcessFiles is present in the config
		DoSomethingWithDirectory (processFiles);
	}



[![Bitdeli Badge](https://d2weczhvl823v0.cloudfront.net/Dynalon/jsonconfig/trend.png)](https://bitdeli.com/free "Bitdeli Badge")

