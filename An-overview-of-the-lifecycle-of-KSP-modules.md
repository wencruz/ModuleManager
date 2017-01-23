Parts contain two things - the logic for the part itself plus a number of modules. Most part functionality is coded in modules, which is useful as it enables mixing together of functionality. For example, all engine and fuel tank functionality are defined in modules in stock KSP. This enables you to create SRBs, which are a combination of both an engine and a tank.

KSP loads parts in a few phases:

### IMMEDIATELY

During this phase, all the .cfg files, including those for PART files, are loaded up by the game. After this, any `KSPAddon` modules that are declared as being run during this phase are run. This includes Module Manager, which gives it a chance to edit the loaded .cfg files.

### The LOADING scene

This is when the Squad monkey is shown up on the screen. 

For each module defined within a part, KSP will:

1. Construct the object and the modules within it
1. Call `OnAwake`
1. Call `OnLoad`, passing in the `ConfigNode` for all the MODULEs as defined in the PART config
1. Then call `GetInfo` on each one
1. Store the constructed part in the `PartLoader` class, within the `AvailablePart` object. For the remainder of this document we'll call it the **available part**.

The available part is then cloned and turned into an icon for display in the VAB. If you want to fiddle with how the icon looks, do it within `GetInfo`.

### Adding a part in the editor (either VAB or SPH)

1. Clone the available part, which results in the modules being cloned also.
1. Call `OnAwake` for each module.
1. Call `OnInitialize` on each module - this is called in the same order as they appear in the stored part.
1. Call `OnStart` on each module, again in the same order.

There's no chance for extra modules to sneak in here, since it's a direct clone and there's no loading.

You then go and attach parts to your ship - how that happens is outside the scope of this document.

### Saving a craft or subassembly in the editor, or a vessel in flight.

The same process is used to save ships in every scene - both in the editors and in flight mode. For every part on the ship, and for each module in the order, they appear in the `PartModuleList` in the part:

1. All the `KSPField(isPersistent=true)` fields are saved to the `ConfigNode`.
1. `OnSave` gets called to save any extra data for the part.

What is saved is generally only the data that can change over time for the part - its state. For example, an engine will store its current throttle setting and a docking clamp will store if it is connected or not.

### Loading a ship

Both in the editor (for saved ships and subassemblies) and in flight mode the same process occurs:

1. The part is cloned from the stored part in the LOADING scene.
1. `OnLoad` is called with the persistent state from the save file for each module *in the same order as in the stored part*.
1. `OnStart` is called.
1. The update loop commences.

There is another callback you can use `OnInitialize`. This doesn't seem to get called consistently (not for the root of the ship) and has no advantage AFAICS over `OnStart`, so I don't use it.



### Adding modules dynamically

Once the part is actually created and running in either the editor or in flight, one could call `AddModule` on the part to add in extra modules at any stage.

The modules will be added to the end of the list, and will get saved in the usual way. However, when the ship gets restored from the save file, the module won't be present in it.