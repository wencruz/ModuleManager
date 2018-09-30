# Module Manager :: Change Log

* 2018-0505: 3.0.7 (sarbian) for KSP 1.4
	+ Fix deprecation
	+ Turn some semi-redundant methods into extensions
	+ Keeps having to reimplement them for every IBasicLogger implementation
	+ Split up prefixing and translating logs for unity
	+ Should be separate classes.
	+ Allow parentheses in value name
	+ Allow spaces in value names
	+ Addresses #107
	+ Fix operators
	+ Addresses #110
	+ Operators are now parsed like commands, removed from the regex.
	+ Fix value assignment with * indexer
	+ Broken in #111 - probably an unusual case but it would have worked
	+ before.
	+ Added tests to ensure that this fixes it.  Tests are not and will
	+ probably never cover all of MMPatchLoader.ModifyNode but useful to add
	+ bugfix cases here as they occur.
	+ Reflection fields should be readonly
	+ Create special GameData subdirectory
	+ It's special
	+ Allow checking needs against directories
	+ If the needs string contains a / it will check for a directory with that
	+ path in GameData.  Notes:
		- PluginData folders are excluded
		- Leading and trailing slashes are allowed
		- Multiple slashes together will be treated as a single slash
		- Comaprison is case sensitive
	+ Require at least one space before the operator (#119)
	+ Fixes wildcards in value names.  If * appears in at the end of value
	+ name without a space it should be interpreted as a wildcard rather than
	+ the multiplication operator
	+ Fix SHA generation for DLL - Fix #120
	+ Make sure TransformFinalBlock is called *after* the last block
