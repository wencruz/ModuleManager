## The problem

With the update to module manager that I'm about to release, there's much better support for adding and removing modules to parts. Before this you would have some issues with save games.

The way KSP handles module loading is somewhat fragile. If you create a part like this:



Of course adding modules to the end of the list would be okay-ish, but you never know when some other mod would go adding its own module to the list, potentially ahead of yours, and messing everything up.

This is now (in the process of being) fixed, however it does open up another set of issues which is what to do with the persistent data 

## An overview of the lifecycle of KSP parts.

