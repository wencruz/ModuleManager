This is an overview of how to manage modules in KSP dynamically, that is to be able to add and remove modules to the game without breaking old saves.

It's targeted really towards mod developers, however I guess could be of interest to others also.

## An overview of the lifecycle of KSP parts.

Parts contain two things - the logic for the part itself, plus a number of modules. Most part functionality is coded in modules, which is useful as it enables mixing together of functionality. For example, all engine and fuel tank functionality are defined in modules in stock KSP. This enables you to create SRBs, which are a combination of both an engine and a tank.

KSP loads parts in a few phases:

### IMMEDIATLY

During this phase all the .cfg files, including those for PART files are loaded up by the game. After this, any `KSPAddon` modules that are declared as being run during this phase are run. This includes module manager, which gives it a chance to edit the loaded .cfg files 

### The LOADING scene

This is when the Squad monkey is shown up on the screen. 

For each module defined within a part, KSP will:
1. Construct the object and the modules within it
2. Call `OnAwake`
3. Call `OnLoad`, passing in the `ConfigNode` for all the MODULEs as defined in the PART config
4. Then call `GetInfo` on each one
5. Clone the part, strip off all functionality except models, and store the model for use as an icon.
6. Store the constructed part in the `PartLoader` class, within the `AvailablePart` object.

### Adding a part in the editor (either VAB or SPH)



## The problem

With the update to module manager that I'm about to release, there's much better support for adding and removing modules to parts. Before this you would have some issues with save games.

The way KSP handles module loading is somewhat fragile. If you create a part like this:

````
PART
{
	name = myPart
	// .. some other stuff
	MODULE
	{
		name = module1
		// .. more stuff
	}
	MODULE
	{
		name = module2
		// .. yet more stuff
	}
}
````

Then that exact order of modules (module1, module2) would be expected whenever you reloaded stuff from save files. This includes .craft files, as well as save games.

Using module manager, prior to 2.1.0, if you had a patch like this:

````
@PART[myPart]
{
	@MODULE[module1]
	{
		addedValue = added
	}
}
````

Then when MM did its thing it would modify module1, and then place the module *at the end of the list* so you'd end up with (module2, module1).

Any saves or craft you had prior to this point would then be broken - that is anything saved to the persistence file from a module, any form of persistence state, would not be able to be retrieved. KSP would just plod along anyhow, leaving everything at the settings they began with in the LOADING scene. 

This would cause for example docking ports to think they were detached, when they were actually attached and thus you not to be able to undock ships. All sorts of other obscure issues can occur. It depends very much on the module how exactly this affects it. Stateless modules such as those for FAR it wouldn't matter at all. Statefull ones would be affected to varying degrees.

Previously we've managed by just adding modules to the end of the list. This will work okay-ish - at least the core modules don't end up broken, but you never know when some other mod would go adding its own module to the list, potentially ahead of yours, and messing everything up. 


