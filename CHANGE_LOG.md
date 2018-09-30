# Module Manager :: Change Log

* 2016-0214: 2.6.18 (sarbian) for KSP 1.0.5
	+ Update README.md
* 2016-0110: 2.6.17 (sarbian) for KSP 1.0.5
	+ Prevents nightingale from trying to breaking some stuff. Fix #44
	+ Add a warning for KSP build 1.0.5.1024
	+ Logging the Exception may be smarter
	+ Store SHA for each cfg and log added/changed/removed cfg
* 2016-0101: 2.6.16 (sarbian) for KSP 1.0.5
	+ Added code for !key,* = DEL and fixed group nr. 5
	+ Bumped Assembly Version
* 2015-1231: 2.6.15 (sarbian) for KSP 1.0.5
	+ Fix a bug with #34
	+ remove some tabs
	+ Comments are nice, let s update them
* 2015-1231: 2.6.14 (sarbian) for KSP 1.0.5
	+ Implemented @key,* = something to resolve #37
	+ Implemented Vector Editing, using @key,*[0]
	+ There are options for editing all keys [*] and specifing a seperator
	+ (defaults to ,): [0, ] (here we use a space as the seperator). Math also
	+ works.
	+ Fixed the Regex
	+ Bumped AssemblyVersion
* 2015-1109: 2.6.13 (sarbian) for KSP 1.0.5
	+ 2.6.13 - let's just change the number and pretend all is fine for 1.0.5
* 2015-1104: 2.6.12 (sarbian) for KSP 1.0.4
	+ No changelog provided
* 2015-0917: 2.6.11 (sarbian) for KSP 1.0.4
	+ Added try/catch to FileSHA.
* 2015-0914: 2.6.10 (sarbian) for KSP 1.0.4
	+ Return empty string if a character-separated list has fewer than the
	+ requested number of elements.
	+ Update version so sarbian can merge.
* 2015-0905: 2.6.9 (sarbian) for KSP 1.0.4
	+ Prevents NullReferenceException when saving the cache to pause the
	+ loading
	+ 30 FPS patching
* 2015-0829: 2.6.8 (sarbian) for KSP 1.0.4
	+ Fix a bug with nested :NEEDS when the top node also used a :NEEDS
* 2015-0804: 2.6.7 (sarbian) for KSP 1.0.4
	+ No changelog provided
* 2015-0625: 2.6.6 (sarbian) for KSP 1.0.4
	+ Add a Quick Reload for ALT F11 menu (skip PartDatabase.cfg generation)
	+ Ignore the cache (and force a PartDatabase.cfg generation) on KSP
* 2015-0523: 2.6.5 (sarbian) for KSP 1.0.3
	+ KSP "-nyan-nyan" option detection for the true believers
	+ Clear the partDatabase if the cache is expired
	+ Display the useful log info even if we use the cache
	+ Do not use the cache if the techtree cache is not present
	+ Format and cleanup
* 2015-0514: 2.6.4 (sarbian) for KSP 1.0.2
	+ Improve the loaded mod listing
	+ Do not change the Tech & physic file patch if they are already OK
	+ Make the error messages more consistent
* 2015-0504: 2.6.3 (sarbian) for KSP 1.0.2
	+ Count the error for math operations
* 2015-0429: 2.6.2 (sarbian) for KSP 1.0.2
	+ No changelog provided
* 2015-0427: 2.6.1 (sarbian) for KSP 1.0.2
	+ No changelog provided
* 2015-0401: 2.6.0 (sarbian) for KSP 1.0.2
	+ No changelog provided
* 2015-0325: 2.5.13 (sarbian) for KSP 1.0.1
	+ DB Corruption check code kept in case of need
	+ Change for #28
	+ Bullet proof PrettyPrint
	+ Remove debug stuff
	+ Forgot 2 lines
	+ Reformat
	+ < and > for value HAS check ( #mass[<100]  ~mass[>100] )
	+ operator for nodes to copy-paste whole nodes
* 2015-0223: 2.5.12 (sarbian) for KSP 1.0.1
	+ No changelog provided
* 2015-0217: 2.5.10 (sarbian) for KSP 1.0.1
	+ Fix the NODE,*:HAS[xxxx] reported by NathanKell
	+ More exception hunting and poor man debuging
* 2015-0427: 2.2.0 (sarbian) for KSP 1.0.1
	+ No changelog provided
* 2014-0522: 2.1.5 (sarbian) for KSP 0.23.5
	+ Fix a bug when inserting a name less node
* 2014-0518: 2.1.4 (Swamp-Ig) for KSP 0.23.5
	+ Allow GameData subdir in NEEDS / BEFORE / AFTER
* 2014-0517: 2.1.3 (Swamp-Ig) for KSP 0.23.5
	+ Removed non-essential backups
	+ Improvements for if multiple copies of the same version are installed.
* 2014-0510: 2.1.2 (Swamp-Ig) for KSP 0.23.5
	+ Lots of improvements and bug fixes in this release.
	+ [Full details here](http://forum.kerbalspaceprogram.com/threads/55219-Module-Manager-2-1-0-%28May-04%29-please-read-the-orange-text-in-first-post?p=1149933&viewfull=1#post1149933)
* 2014-0504: 2.0.9 (Swamp-Ig) for KSP 0.23.5
	+ So I've done my own enhancements to module manager.
	+ They seem to work pretty nice!  I'd really appreciate ppl testing it out on their various MM files prior to me pushing it back to the MM repo.
	+ Any issues you have - take them up with me not with sarbian
	+ Features:
	+ Order Preserving
		- now preserves the original order, both for nodes and for values.
	+ Use of NEEDS
```
PART:NEEDS[RealFuels] {
    name = dummyPartIgnore
    module = Part
    DOG {
        name = First
        key1:NEEDS[ProceduralParts|StretchySRB] = Original Value
        key2:NEEDS[!RealFuels] = Some other value
    }
}
```
	+ In the above, the part will only be defined if you have RealFuels loaded. You can do this on keys, values, patches, anywhere.
	+ key1 will be defined if (ProceduralParts OR StretchySRB are loaded) and RealFuels is NOT loaded.  You can still use , and it is treated like &. Not has highest precedence, then or, then and.
	+ Define an insertion point for any nodes or values
```
@PART[dummyPartIgnore]
{
    DOG {
        name = AddLast
        string = Will insert at the end
    }
    DOG,0 {
        name = AddFirst
        string = Will insert at the beginning
    }
    @DOG,0 { // Edits node zero as before, but ends up editing the above node
        string,0 = Insert before the string already at the beginning
    }          
}
```
	+ The index after the comma is where it will be inserted, this is relative to other nodes or values with the same name.
	+ Regexp replaces
```
@PART[dummyPartIgnore]
{
    @DOG[First] 
    {
        @string ^= :^.*$:First dog edit $& in place
    }
}
```
	+ Breaking this down, the first character defines the separator, the section between the first : and the second : is the match string, and the section following the second : is the replacement.
	+ For details on how to use regexp [see the documentation here](http://msdn.microsoft.com/en-us/library/hs600312%28v=vs.110%29.aspx)
