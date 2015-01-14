#Module Manager Handbook
This section covers the most basic applications of Module Manager, which tend to be the most asked questions in the [Module Manager Official Thread](http://forum.kerbalspaceprogram.com/threads/55219). It goes through the main operations in a quick and simple way.
For a more detailed description, see [Module Manager Syntax](https://github.com/sarbian/ModuleManager/wiki/Module%20Manager%20Syntax).

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
 - `#` for including "nodes with values" in filter
 - `~` for excluding "nodes with values" from filter
 - `:HAS[<node>]` for searching only files that have <node> in filter
 - `:NEEDS[<modname>]` for searching only files that mess with certain mod.
- Additional
 - `&` or `,` for "AND"
 - `|` for "OR"
 - `:Final` forces the patch to be applied lastly (in case multiple files edit the same node)


###Examples and details:

- Editing and creating:
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
        @key,0 = 0 390 // Edits the FIRST variable "key" from atmosphereCurve
        @key,1 = 1 320 // Edits the SECOND variable "key" from atmosphereCurve
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
The code above is explained on each of its lines, but let's chew it even more:

If you make a .cfg file with an entry that looks like this:
```
PART
{
  name = myPart
  ...(stuff)
}
```
You're defining a new part named 'myPart". Then, if another .cfg file somewhere does this:
```
@PART[myPart]
{
  ...(stuff)
}
```
That is saying: **"at the PART named 'myPart', edit the following additional stuff..."**.



If there are two or more variables with the same name, you can refer to them like this:

`@example,0 = <...>` finds the first "example" variable or on the list (this is the same as "@example = <...>")

`@example,1 = <...>` finds the second one.

`@example,2 = <...>` finds the third, and so on.



#WIP