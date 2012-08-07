About
====================

JsonConfig is a simple to use configuration library, allowing JSON based config
files for your C#/.NET application instead of cumbersome
web.config/application.config xml files.

It is based on JsonFX and C# 4.0 dynamic feature. Allows putting your programs
config file into .JSON files, where a default config can be embedded as a
resource or put in the (web-)application folder.

Configfiles are inherited from during runtime, so different configs are merged
into a single dynamic object (Example: Defaultconfig is overriden by user
config). Config is accesible as a dynamic object.

Example
=====================

Put your default settings (standard settings if user specifies no configs) into
default.conf.json file and embed it as a resource.

Then create a settings.conf.json and put it into the same folder as your .exe or, for
asp.net apps, in the same folder as your web.config.

At runtime, settings.conf will overwrite the specified default values from
default.conf.json.

Example usage:

	TODO

Current state
=====================
Under development, documentation currently not present.
