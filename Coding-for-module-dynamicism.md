
## The problem

With the update to module manager that I'm about to release, there's much better support for adding and removing modules to parts. Before this patch you would have some issues with save games - any change to the order of modules defined in a part would result in broken save games.

Of course adding modules to the end of the list would be okay-ish, but you never know when some other mod would go adding its own module to the list, potentially ahead of yours, and messing everything up.

This is now (in the process of being) fixed, however it does open up another set of issues which is what to do with the persistent data 

## An overview of the lifecycle of KSP parts.

