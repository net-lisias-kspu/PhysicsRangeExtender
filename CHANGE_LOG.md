# Physics Range Extender :: Change Log

* 2018-1021: 1.8.0 (jrodrigv) for KSP 1.5.1
	+ v1.8.0
	+ Recompiled for KSP 1.5.1
	+ Default range reduced to 50 km
	+ UI simplifed. Removing options that were not used
	+ Added button to disable de mod.
	+ Added message to warn the user when the flickering fix is activated.
* 2018-1010: 1.7.0.1 (Lisias) for KSP 1.4.3
	+ Adding [KSPe](https://www.github.com/net-lisias-ksp/KSPAPIExtensions) Hard Dependency 
		- Moving settings file into <KSP_ROOT>/PluginData where God intended it to be.
		- Logging Facilities 
	+ Some code optimizations for better performance. 
* 2018-0620: 1.7.0 (jrodrigv) for KSP 1.4.3
	+ v1.7.0
	+ Recompiled and upgraded for KSP 1.4.3
	+ Adding option to extend terrain loading distance (thanks to Gedas). So there is no need of statics anymore!
	+ Flickering fixed at 99% after applying a dynamic update of the near clip plane of the main camera.
	+ Landed range removed, only one range is needed now :)
* 2018-0311: 1.6.0 (jrodrigv) for KSP 1.4
	+ v1.6.0
	+ Recompiled and upgraded for KSP 1.4
* 2017-0916: 1.5.0 (jrodrigv) for KSP 1.3
	+ Fixes de-orbit vessels when entering Global Range distance.
	+ Unload landed or splashed vessels to prevent its destruction when the active vessel is about to change its reference frame (from rotation to inertial).
	+ If the user decide to increase the range while there are orbiting vessels unloaded.
	+ The new range will not be applied to these vessels to prevent de-orbiting. The game should be saved and loaded again for safety.
	+ A new toggle button has been added to the UI allowing to "Force" the ranges, overriding all the safety checks.
	+ (This may cause undesired effects like orbiting vessels de-orbit or landed vessels destruction)
	+ New screen messages have been added to inform the user about events.
	+ PRE icon visible in all scenes
* 2017-0528: 1.4.0 (jrodrigv) for KSP 1.3
	+ v1.4.0
	+ Recompiled for KSP 1.3
* 2017-0514: 1.3.0 (jrodrigv) for KSP 1.2.X
	+ v1.3.0
	+ Added GUI to enable/disable the mod and adjust the ranges
	+ Several code improvements.
	+ Added license and readme files
* 2017-0430: 1.2.0 (jrodrigv) for KSP 1.2.X
	+ Added settings.cfg to allow adjust the ranges.
	+ RangeForLandedVessels: parameter that can be reduced in order to avoid landed vessels to crash into the terrain. (10 km by default),
	+ GlobalRange: For all scenarios different than Landed. (100 km by default)
* 2017-0403: 1.1.0 (jrodrigv) for KSP 1.2.X
	+ Range extended to 2000 km
	+ Recompiled for KSP 1.2.9
* 2017-0320: 1.0.0 (jrodrigv) for KSP 1.2.2
	+ No changelog provided
