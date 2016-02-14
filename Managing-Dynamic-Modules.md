This is an overview of how to manage modules in KSP dynamically, that is to be able to add and remove modules to the game without breaking old saves.

It's targeted really towards mod developers, however I guess could be of interest to others also.

## In a nutshell

* If you create a mod, and you add and remove modules dynamically (as in in the Editor or Flight scenes) then please save `MM_DYNAMIC=true` in your module state.
* If you create a mod, and you add modules to parts with MM or in the LOADING scene, and you want to know when your module is added a vessel from a pre-existing save, check for `MM_REINITIALIZE=true` in `OnLoad` for your module.
* If you have state that changes over the lifetime of a vessel, and you want to know when your mod has been deleted and reinstalled, check for `MM_RESTORED=true` in `OnLoad` for your module.

## The problem

With module manager 2.1.0 and above, there's much better support for adding and removing modules to parts. Before this you would have some issues with save games.

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

This caused various difficult to identify errors - for example docking ports save their 'docked' or 'undocked' state in the save, so after any module order change they would think they were detached, when they were actually attached and thus you not to be able to undock ships. All sorts of other obscure issues can occur. It depends very much on the module how exactly this affects it. Stateless modules such as those for FAR it wouldn't matter at all. Statefull ones would be affected to varying degrees. Even if your MM patch didn't directly affect a module, since the order of definition of modules changed then any module defined after that one would not load up properly.

Previously we've managed by just adding modules to the end of the list. This will work okay-ish - at least the core modules don't end up broken, but you never know when some other mod would go adding its own module to the list, potentially ahead of yours, and messing everything up. 

## The fix

The new version of MM has things in place to prevent this:

1. The order of definition of things is no longer changed when you edit (@) them
1. SaveGameFixer runs through all previous save games and fixes them whenever KSP starts up

Both of these steps were needed - this makes it robust to any config change.

SaveGameFixer runs in the main menu scene, so the version of the part database it works with is whatever is the result of any MM patches. It will run through all the saved games and saved craft files and perform the following actions:

1. *Modules reordered:* either in the PART file or by a MM patch. Reorder the modules in the save file to match the part in the part database
1. *Module added in flight or VAB:* If there's any modules present in the save file, present in memory (as in a mod exists that defines that module), but not in the part database the stored state will be discarded. This allows you to add dynamic modules to the end of the list (dynamic module definition to follow)
1. *Mod with module deleted:* If there's modules present in the save, not present in memory, and not in the part database then these modules's persistence data will be stored inside the save, and the save will also be backed up. This occurs when a mod is uninstalled with a module still being in use on a ship.
1. *Mod from above reinstalled:* If there's any backups from above now present in the part, they will be restored from the internal vessel  and the save will be backed up
1. *Part not found:* KSP will just destroy any ship with a missing part, SaveGameFixer will back up the save file (but not the quicksave) to protect from this issue.

As a mod developer, there's a couple of things you can do to make this run smoothly:

### Dynamic modules

Dynamic modules are added to a part at runtime, that is either in flight or in the VAB. Generally SaveGameFixer will detect this and silently discard the persisted data, which is what would have happened previously.

In the case where your mod gets deleted, SaveGameFixer can't know if this was just because it's a dynamic module, or because it's because your mod is deleted. 

To help this along, if you have a dynamic module then please do something like this:

````c#
    public override void OnSave (ConfigNode node)
    {
        node.AddValue("MM_DYNAMIC", "true");
    }
````

This will avoid the backup being created, which saves time later.

### Reinitialized modules

If you add a new mod or MM patch that adds a module to an existing part, MM will detect this and create an empty persisted state for the module:

````
    MODULE
    {
        name = YourModuleNameHere
        MM_REINITIALIZE = true
    }
````

If you want to do something fancy with this condition - say initializing state or something, then you can detect the flag like this:

````c#
    public override void OnLoad(ConfigNode node)
    {
        if (node.GetValue("MM_REINITIALIZE") != null)
        {
            // Do whatever
        }
    }
````

### Reload from module state backup

The final case is when a user has uninstalled your mod, and then reinstalled it again. The module state will be saved and restored when this happens. The contents of the module state will be exactly as it was at the time of uninstallation, but there'll be an extra flag MM_RESTORED.

Depending on your mod, you might want to reinitialize or otherwise do something special. 


## Conclusion

The new features of save game fixing make MM and modding far more flexible. Most of the time the behaviour is what you'd expect it to be, but it's a good idea to have an awareness of these features.