# Applying patches in order

There are many cases when you want to apply patches from the same mod, different mods, or different files, but in a specific order. This can be challenging without a clear understanding of exactly how module manager applies patches. This document attempts to clear up the misunderstandings so mod-makers and patch-makers can write robust patches for their mods.

## Overview
You can control how module manager applies patches through these five directives, that are applied at the top-level node. Only **ONE** of these directives can be applied to a specific top-level patch.
* :FIRST
* :BEFORE[_modname_]
* :FOR[_modname_]
* :AFTER[_modname_]
* :FINAL 

Patches are applied in the following order:
1. Nodes with no operator ('insert') are loaded by the KSP GameDatabase first.
2. Patches for _modname_ values in NEEDS, BEFORE, AFTER that don't exist are removed.
3. All patches with :FIRST are applied.
4. All patches without an ordering directive (:FIRST, :BEFORE, :FOR, :AFTER, :LAST, :FINAL) are applied.
5. For each item in the Unicode-sorted list of _modname_ values:
    * All patches with :BEFORE are applied
    * All patches with :FOR are applied
    * All patches with :AFTER are applied
6. All patches with :FINAL are applied

## Nodes with no operator
At the top-level node, INSERT operations take place prior to module manager being run. So a new node is created before any patches ever execute. 

Example:

    PART
    {
        name = MyNewPart
    }

This part will be loaded and available for all patches with all directives.

## Valid [_modname_] values
Module Manager generate list of **case-insensitive** _modname_ values that will work in NEEDS, BEFORE, AFTER by doing the following:
* Scans all the loaded DLLs and adds _modname_.dll to the list of loaded mods (just the _modname_ portion)
* Scans all the configs for properly configured FOR[_modname_] and add _modname_ to the list of loaded mods
* Scans the GameData directory and adds _modname_ to the list of loaded mods, with white space characters removed from the name of the folder (space, tab, and several others defined by Unicode/.NET).

Module manager will remove patches with NEEDS, BEFORE, or AFTER that don't have a valid _modname_.

Example: Any of these three will allow you to reference MyCoolMod:
    
>     GameData/ModMakerName/Mod/MyCoolMod.dll

>     %PART:FOR[MyCoolMod]
>     {
>        name = MyNewPart
>     }

>     GameData/MyCoolMod

Because of this behavior of ModuleManager, it is encouraged for mod makers who put their mods in subfolders at least specify one patch with a :FOR, so that other modders can reference that mod.


## The :FIRST directive
These patches will run after the game database is loaded, but before any other patches.
All :FIRST patches will be applied in alphabetical order by folder / subfolder / filename.

Example:
This part copy will never happen because the part was deleted in the FIRST pass.

>     PART
>     {
>         name = MyNewPart
>         valueEdit1 = 1
>     }
>     
>     !PART[MyNewPart]:FIRST {}
>     
>     +PART[MyNewPart]
>     {  
>         name = MyNewPart2
>         @valueEdit1 = 2
>     }

## The :BEFORE[_modname_], :FOR[_modname_], and :AFTER[_modname_] directives
These directives cause patches with them specified to be loaded during the pass for the specific mod _modname_.
Patches are applied by going through each _modname_ in alphabetical order, and applying the all BEFORE, then all FOR, then all AFTER patches. Within a specific patch group, patches will be applied in alphabetical order by folder / subfolder / filename.

Example:

I have this part:
    PART
    {
        name = MyCoolPart
        value = ORIGINAL
    }

I have some patches in two mods:
MOD00/PATCH00.cfg

    @PART[MyCoolPart]
    {
        @value = PATCH00
    }

MOD00/PATCH01.cfg

    @PART[MyCoolPart]:BEFORE[MOD01]
    {
        @value = PATCH01
    }

MOD00/PATCH02.cfg

    @PART[MyCoolPart]:AFTER[MOD00]
    {
        @value = PATCH02
    }

MOD00/PATCH03.cfg

    @PART[MyCoolPart]:FOR[MOD00]
    {
        @value = PATCH03
    }

MOD05/PATCHMOD00FROMMOD05.cfg

    @PART[MyCoolPart]:FOR[MOD00]
    {
        @value = PATCHMOD00FROMMOD05
    }

When module manager processes these:
- The patch in PATCH01.cfg is deleted because BEFORE can't be satifised (no MOD01)
- value is set to PATCH00 in the 'legacy' pass (after :FIRST but before others)
- value is then set to PATCH03 in the FOR[MOD00] pass
- value is then set to PATCHMOD00FROMMOD05 in the FOR[MOD05] pass (alphabetical order)
- value is finally set to PATCH02 in the AFTER[MOD00] pass

Figuring out what overwrote what can be a challenge in module manager, especially with mods that alter certain things globally on all PART nodes, for example.

## The :FINAL directive
This pass should be reserved, if possible, for end-user patches only, not distributed mods.
This runs after all FIRST, _no directive_, and BEFORE/FOR/AFTER patches have completed.
Patches will be applied in alphabetical order by folder / subfolder / filename.
