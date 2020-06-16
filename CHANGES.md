# Module Manager /L Experimental :: Changes
*Lasciate ogne speranza, voi ch'intrate*
- - -

* 2020-0616: 4.1.3.2 (Lisias) for KSP >= 1.3.1
	+ A nasty mishap from a old merge (dated 2018-11) was detected and fixed.
		- See Issue [#4](https://github.com/net-lisias-ksp/ModuleManager/issues/4). 
* 2020-0526: 4.1.3.1 (Lisias) for KSP >= 1.3.1
	+ Adding a nice 'Houston' GUI message
	+ Merging upstream updates:
		- Adding an Exception interceptor to catch `ReflectionTypeLoadException` and properly blame DLLs
		- Cleanup the InterceptLogHandler, remove double logging and avoid any risk of throwing more
	+ These things were reworked to keep them compatible with previous KSP versions.
