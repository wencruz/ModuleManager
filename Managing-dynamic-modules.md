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
1. Call `OnAwake`
1. Call `OnLoad`, passing in the `ConfigNode` for all the MODULEs as defined in the PART config
1. Then call `GetInfo` on each one
1. Clone the part, strip off all functionality except models, and store the model for use as an icon.
1. Store the constructed part in the `PartLoader` class, within the `AvailablePart` object.

### Adding a part in the editor (either VAB or SPH)

1. Clone the part that is the result from above
1. Call `OnInitialize` on each module - this is called in the same order as they appear in the stored part
1. Call `OnStart` on each module, again in the same order

There's no chance for extra modules to sneak in here, since it's a direct clone and there's no loading.

Once the part is actually created and running, one could call `AddModule` on the part to add in extra modules at any stage. This will add modules at the end of the list.

You then go an attach parts to your ship - but how that happens is outside the scope of this document.

### Saving a ship (or a subassembly)

The same process is used to save ships in every scene - both in the editors and in flight mode. For every part on the ship, and for each module in the part (in order):

1. `OnSave` gets called to save the data for the part.

This is generally only the data that can change over time for the part - its state. For example an engine will store its current throttle setting and a docking clamp will store if it is connected.

### Loading a ship

Both in the editor (for saved ships and subassemblies) and in flight mode the same process occurs:

1. The part is cloned from the stored part in the LOADING scene
1. `OnLoad` is called with the persistent state from the save file for each module *in the same order as in the stored part*



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


