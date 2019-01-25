# Module Manager /L Experimental :: Changes
*Lasciate ogne speranza, voi ch'intrate*
- - -

* 2019-0125: 4.0.0.2 (Lisias) for KSP >= 1.3.1
	+ Merging upstream updates:
		- [blowfish](https://forum.kerbalspaceprogram.com/index.php?/profile/119688-blowfish/) worked his magic once more and now MM does the patching while the game loads the models and textures.
		- Fix tech tree and modded physics
	+ (my) Fixes to the upstream:
		- Fixing the Logging system, restoring the (sane) previous behaviour. The new logs were preserved, and can be found on `<KSP_ROOT>/Logs/ModuleManager`
		- Allowing "stock" compatibility to 1.3.1 by avoiding >= 1.4 specifics #hurray :)
		- Monitoring changes to TechTree and Physics
		- Preventing hijacking them when another Add'On changes them.
			- They are set up only **one** at first time Space Center is loaded. From there, it only logs if they were changed.
	+ Stating **Official** support for KSP 1.3.1 :)
* 2019-0110: 3.1.3.1 (Lisias) for KSP >= 1.4.1
	+ Merging 1.6.0 DragCube workaround from uptream
	+ Certifying for use on 1.5.x and 1.6.x series
	+ Bumping up version to catch upstream's
* 2018-1112: 3.1.1.1 (Lisias) for KSP 1.4; 1.5
	+ Adding KSPe logging facilities
	+ Syncing source with upstream latest fixes.  
		- more Internal code improvement by @blowfish  
* Older news
	+  Declaring this thing **EXPERIMENTAL**.
		- I will properly maintain it, but it still Experimental (and non Standard)
	+ We have moved to the properly maintained /L Division! :)
	+ Moving the datafiles	 to <KSP_ROOT>/PluginData , where God intended it to be.
	+ Using KSPe facilities
		- (some) Logging
		- File System and Data Files
