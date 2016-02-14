#Module Manager Handbook
This section covers the most basic applications of Module Manager, which tend to be the most asked questions in the [Module Manager Official Thread](http://forum.kerbalspaceprogram.com/threads/55219). It goes through the main operations in a quick and simple way.
For a more detailed description, see [Module Manager Syntax](https://github.com/sarbian/ModuleManager/wiki/Module%20Manager%20Syntax).

For future reference in this handbook, keep in mind of the following nomenclatures:
- NODE
 - Their content is written between curly brackets. Examples:
 - `MODULE { }`
 - `RESOURCE { }`
 - `PROP { }`
 - etc.

- Key
 - Items that can have a value after it. Examples:
 - `name = mk2LanderCabin`
 - `maxThrust = 1500`
 - `description = The mobile processing lab was developed to (...)`
 - etc.

##Operations

###Common Syntaxes:

- Operators
 - "nothing", for creating a new node
 - `@` for edit
 - `+` or `$` for copy
 - `-` or `!` for delete
 - `%` for edit-or-create.

- Filters
 - `*` for any number of alphanumeric chars
 - `?` for any single alphanumeric character. This is also applied in case of "space" or special chars.
 - `@` for including nodes in filter
 - `-` or `!` for excluding nodes from filter
 - `#` for including keys in filter
 - `~` for excluding keys from filter
 - `:HAS[<node>]` for searching only files that have <node> in filter
 - `:NEEDS[<modname>]` Patch is applied only if given mod is installed.

- Additional
 - `&` or `,` for "AND"
 - `|` for "OR"
 - `:Final` forces the patch to be applied lastly (in case multiple files edit the same node)


###Examples and details:
- Creating or editing using Operators:

```
@PART[SomePart] // Edit a PART node named "SomePart".
{
    @mass = 0.625 // change SomePart's mass to 0.625
    @description = SomePart: now uses Xenon! // Changes the value from the "description" item. In this case, a text.

    @MODULE[ModuleEngines] // Edit SomePart's node MODULE named "ModuleEngines"
    {
        @maxThrust = 2.25  // Changes maxThrust to 225

        @PROPELLANT[LiquidFuel] // Edit SomePart's node PROPELLANT named "LiquidFuel"
        {
            @name = XenonGas // Changes the PROPELLANT node name from LiquidFuel to XenonGas.
            @ratio = 1.0 // Changes the ratio value.
        }

        @atmosphereCurve // Edit SomePart's node atmosphereCurve. Note that this node doesn't have a name.
        {
            @key,0 = 0 390 // Edits the FIRST "key" Key from the "atmosphereCurve"
            @key,1 = 1 320 // Edits the SECOND "key" Key from the "atmosphereCurve" property
        }

        !PROPELLANT[Oxidizer] {} // Removes the node PROPELLANT named "Oxidizer" from the PART.
    }

    RESOURCE // Creates a new node RESOURCE in the PART.
    {
        name = ElectricCharge // Adds a name to the node RESOURCE
        amount = 100 // Adds "amount" and its value to this node
        maxAmount = 100 Adds "maxAmount" and its value to this node
    }
}
```
The code above have explanations on each of its lines, but let's chew it even more:

If you make a .cfg file with an entry that looks like this:
>```
PART
{
  name = myPart
  ...(stuff)
}
```
You're defining a new part named "myPart". Then, if another .cfg file somewhere does this:

<br>
>```
@PART[myPart]
{
  ...(stuff)
}
```
That is saying: **"at the PART named 'myPart', edit the following additional stuff..."**.
If you don't put the `@` operator before the NODE, instead of editing an existent one, you will create a new one:

<br>
>```
PART[myPart]
{
 ...(stuff)
}
```
This way, now you have two PARTs named "myPart".


***

<br>
##Filtering by numbers:
It's also possible to filter Nodes and Keys by numbers. This is useful when there's multiple and nameless (or under the same name) nodes and Keys on a config file.

- Keys:
If there are two or more Keys with the same name, you can refer to them like this:

`@example,0 = <...>` finds the first "example" Key or on the list (this is the same as `@example = <...>`)

`@example,1 = <...>` finds the second one.

`@example,2 = <...>` finds the third, and so on.

`@example,* = <...>` finds all the "example" Keys, and edits all of them.

`@example,-1 = <...>` finds the last "example" Key.

The same thing works for `!example,0`, etc.

<br>
- Nodes:
The same is applied to nodes without names, as follows:


`!EXAMPLE,0 {}`
Looks for the first "EXAMPLE" node in the section and deletes it.

>```
@EXAMPLE,*
{
<...>
}
```
Looks and edits all the "EXAMPLE" nodes in the section.

<br>
>```
@MODULE[Example],1
{
<...>
}
```
Looks for all the "Example" MODULES. Filters the second one and edits it. Note that this is a named MODULE, but this doesn't prevent you from filtering them by numbers.


***

<br>
##Editing Multiple Parts:
You can apply changes to multiple parts at the same time, using the `*` filter. Examples:

- Specific names

>```
@PART[B9_*]
{
  ...(stuff)
}
```
This will edit all the PART nodes that has a name **beginning with "B9_", and have anything else after it**.  

<br>
- Specific nodes

>```
@PART[*]:HAS[@MODULE[ModuleEngines]]
{
  ...(stuff)
}
```
This will look for all PART nodes, but will filter for only those who contain `ModuleEngines` MODULE.

<br>
>```
@PART[*]:HAS[!MODULE[ModuleCommand]]
{
  ...(stuff)
}
```
Like the previous one, this will look for all PART nodes, but will filter for only those who don't contain `ModuleCommand` MODULE.

<br>
- Specific Keys

>```
@PART[*]:HAS[#category[Utility]]
{
  ...(stuff)
}
```
This will look for all PART nodes, and filter for those who have a `category = Utility` Key. Note that this category must not be inside any other node. It must be directly inside the mentioned PART.

<br>
>```
@PART[*]:HAS[~TechRequired[]]
{
  ...(stuff)
}
```
This will look for all PARTs that DON'T have any `TechRequired =` Key.

<br>
- Specific Configuration

>```
@PART[*]:HAS[@RESOURCE[MonoPropellant]:HAS[#maxAmount[750]]]
{
  ...(stuff)
}
```
This will look for all PARTs that have the `MonoPropellant` RESOURCE. And from these, it will filter again for only those RESOURCE nodes that have a `maxAmount = 750` Key.

<br>
>```
@PART[*]:HAS[@MODULE[ModuleEngines]:HAS[@PROPELLANT[XenonGas]]]
{
  ...(stuff)
}
This will look for all PARTs who have a `ModuleEngines` MODULE using XenonGas as a propellant.

<br>
- Combined Search

>```
@PART[*]:HAS[@MODULE[ModuleEngines] , @RESOURCE[SolidFuel]] 
{
  ...(stuff)
}
```
This filters for all PARTs who have a `ModuleEngines` MODULE and have a `SolidFuel` RESOURCE at the same time. (Space added for clarity)

<br>

>```
@PART[*]:HAS[  @MODULE[ModuleEngines] :HAS [ @PROPELLANT[XenonGas] , @PROPELLANT[ElectricCharge] ]  ]
{
  ...(stuff)
}
```
This goes for all PART thats have `ModuleEngines` containing `XenonGas`, and `ElectricCharge` at the same time. (Space added for clarity)

<br>
- Even deeper:

>```
@PART[*]:HAS[!RESOURCE[ElectricCharge],@RESOURCE[*]]
{
  ...(stuff)
}
```
All PARTs without ElectricCharge as a ressource but with any other.


***

<br>
##Some useful examples:

>```
@PART[*]:HAS[~TechRequired[]]:Final
{
	TechRequired=advScienceTech
}
```
Adds a tech level to all PARTs who don't have any.

<br>

>```
@PART[*]:HAS[@MODULE[ModuleCommand],!MODULE[MechJebCore]]:Final
{    
    MODULE
    {
        name = MechJebCore
        MechJebLocalSettings 
        {
            MechJebModuleCustomWindowEditor {unlockTechs = flightControl}
            MechJebModuleSmartASS {unlockTechs = flightControl}
            MechJebModuleManeuverPlanner {unlockTechs = advFlightControl}
            MechJebModuleNodeEditor {unlockTechs = advFlightControl}
            MechJebModuleTranslatron {unlockTechs = advFlightControl}
            MechJebModuleWarpHelper {unlockTechs = advFlightControl}
            MechJebModuleAttitudeAdjustment {unlockTechs = advFlightControl}
            MechJebModuleThrustWindow {unlockTechs = advFlightControl}
            MechJebModuleRCSBalancerWindow {unlockTechs = advFlightControl}
            MechJebModuleRoverWindow {unlockTechs = fieldScience}
            MechJebModuleAscentGuidance {unlockTechs = unmannedTech}
            MechJebModuleLandingGuidance {unlockTechs = unmannedTech}
            MechJebModuleSpaceplaneGuidance {unlockTechs = unmannedTech}
            MechJebModuleDockingGuidance {unlockTechs = advUnmanned}
            MechJebModuleRendezvousAutopilotWindow {unlockTechs = advUnmanned}
            MechJebModuleRendezvousGuidance {unlockTechs = advUnmanned}
        }
    }
}
```
Enables MechJeb on all pods and probes. Respects the Tech-tree.

<br>

>```
@EXPERIMENT_DEFINITION[*]:HAS[#id[gravityScan]]
{
    @baseValue = 5
    @scienceCap = 10
}
```
Most examples use PARTs, but it works on other nodes too.