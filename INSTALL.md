# Physics Range Extender /L Unofficial

It extends game physics range, allowing you to switch between vessel that are far away. 


## Installation Instructions

To install, place the GameData folder inside your Kerbal Space Program folder. Optionally, you can also do the same for the PluginData (be careful to do not overwrite your custom settings):

* **REMOVE ANY OLD VERSIONS OF THE PRODUCT BEFORE INSTALLING**, including any other fork:
	+ Delete `<KSP_ROOT>/GameData//PhysicsRangeExtender `
* Extract the package's `GameData` folder into your KSP's root:
	+ `<PACKAGE>/GameData` --> `<KSP_ROOT>/GameData`
* Extract the package's `PluginData` folder (if available) into your KSP's root, taking precautions to do not overwrite your custom settings if this is not what you want to.
	+ `<PACKAGE>/PluginData` --> `<KSP_ROOT>/PluginData`
	+ You can safely ignore this step if you already had installed it previously and didn't deleted your custom configurable files.

The following file layout must be present after installation:

```
<KSP_ROOT>
	[GameData]
		[PhysicsRangeExtender]
			[PluginData]
				[Textures]
					icon.png
				default.cfg
			CHANGE_LOG.md
			LICENSE
			LICENSE.SKL-1_0	
			LICENSE.UN
			NOTICE
			PhysicsRangeExtender.dll
			PhysicsRangeExtender.version
			README.md
		000_KSPe.ddl
		ModuleManager.dll
	[PluginData]
		[PhysicsRangeExtender] <not present until you run it for the fist time>
			settings.cfg 
	KSP.log
	PartDatabase.cfg
	...
```


### Dependencies

* [KSP API Extensions/L](https://github.com/net-lisias-ksp/KSPAPIExtensions)
	+ Hard Dependency - Plugin will not work without it.
	+ Not Included

