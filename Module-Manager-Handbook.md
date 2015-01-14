#Module Manager Handbook
This section covers the most basic applications of Module Manager, which tend to be the most asked questions in the [Module Manager Official Thread](http://forum.kerbalspaceprogram.com/threads/55219). It goes through the main operations in a quick and simple way.
For a more detailed description, see [Module Manager Syntax](https://github.com/sarbian/ModuleManager/wiki/Module%20Manager%20Syntax).

For future reference in this handbook, keep in mind of the following nomenclatures:
- NODE
 - Their content is written between curly brackets. Examples:
 - MODULE { }
 - RESOURCE { }
 - PROP { }
 - etc.

- Variable
 - Items that can have a value after it. Examples:
 - name = mk2LanderCabin
 - maxThrust = 1500
 - description = The mobile processing lab was developed to (...)
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
 - `#` for including variables in filter
 - `~` for excluding variables from filter
 - `:HAS[<node>]` for searching only files that have <node> in filter
 - `:NEEDS[<modname>]` for searching only files that mess with certain mod.
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
            @key,0 = 0 390 // Edits the FIRST variable "key" from the "atmosphereCurve"
            @key,1 = 1 320 // Edits the SECOND variable "key" from the "atmosphereCurve" property
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
```
PART
{
  name = myPart
  ...(stuff)
}
```
You're defining a new part named "myPart". Then, if another .cfg file somewhere does this:
```
@PART[myPart]
{
  ...(stuff)
}
```
That is saying: **"at the PART named 'myPart', edit the following additional stuff..."**.
If you don't put the `@` operator before the NODE, instead of editing an existent one, you will create a new one:
```
PART[myPart]
{
 ...(stuff)
}
```
This way, now you have two PARTs named "myPart".


***


###Filtering by numbers:
It's also possible to filter nodes and variables by numbers. This is useful when there's multiple and nameless (or under the same name) nodes and variables on a config file.

- Variables:
If there are two or more variables with the same name, you can refer to them like this:

`@example,0 = <...>` finds the first "example" variable or on the list (this is the same as `@example = <...>`)

`@example,1 = <...>` finds the second one.

`@example,2 = <...>` finds the third, and so on.

`@example,* = <...>` finds all the "example" variables, and edits all of them.

`@example,-1 = <...>` finds the last "example" variable.

The same thing works for `!example,0`, etc.

- Nodes:
The same is applied to nodes without names, as follows:


`!EXAMPLE,0 {}`
Looks for the first "EXAMPLE" node in the section and deletes it.

```
@EXAMPLE,*
{
<...>
}
```
Looks and edits all the "EXAMPLE" nodes in the section.

```
@MODULE[Example],1
{
<...>
}
```
Looks for all the "Example" MODULES. Filters the second one and edits it. Note that this is a named MODULE, but this doesn't prevent you from filtering them by numbers.



***


###Editing Multiple Parts:


#WIP
Do not edit until the sign above has been removed.