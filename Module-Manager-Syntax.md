#A primer on how KSP configures itself

KSP uses ConfigNodes extensively for all aspects of configuration. The main way in which modders interact with these is in the configuration of parts. [There's a fair bit of documentation about this elsewhere](http://wiki.kerbalspaceprogram.com/wiki/User:Greys), but for the purposes of this document we will define a few things:

```
// This is a top level node. Note the name with curly brackets afterwards.
PART
{
    // This is the name value. It's just a standard name/value pair, but any of these named 'name'
    // is treated a bit special by MM
    name = myPart

    // This is another value. Note name = value . You can't have a name without a value,
    // but you can have a value without a name (treated as the empty string for name )
    module = Part

    // This is a node named 'MODULE'. It's a collection of further values and modules
    MODULE
    {
        name = ModuleEngines
    }
}

// This is a patch. Note the prefix - this will be one of  '@' for edit, '+' or '$' for copy,
// '-' or '!' for delete, '%' to edit-or-create. A patch takes some pre-existing top level node, and modifies it
@PART[myPart]
{
    // Edit the value with name 'module'
    @module = PartEnhanced

    // Delete all 'MODULE' nodes.
    -MODULE,* { }
}
```

###Top level Node
(Soon)
###Selection
(Soon)
###Patch order
(Soon)
###Dependency
(Soon)
(copy/move some of the next section)

##Mod dependency checking

You can now put a NEEDS section on any section of your file. This isn't just MM patches, it can be literally anywhere - on values, on nodes, on patches, wherever you like. (this enhanced behaviour is new)

An example:

```
PART:NEEDS[RealFuels|ModularFuelSystem]
{
    name = myPartName

    description:NEEDS[RealFuels&!ModularFuelSystem] = This part is used in real fuels
    description:NEEDS[ModularFuelSystem&!RealFuels] = This part is used in modular fuel system
}
```

So what this means is the part will only be defined if RealFuels OR ModularFuelSystem is present.
There's two alternatives for the description field, based on what's present.

This is handy if you're a mod developer and you want to create parts that vary a bit depending on what's installed.

The stuff within the needs section is based on either:
* A plugin .dll with the same assembly name. 
* A subdirectory name under GameData.
* A FOR[Blah] defined would allow NEEDS[Blah]

As this uses the assembly name (which is compiled into the DLL)  so you'd always get ModuleManager even if you rename the dll. It's usually going to be the same as the DLL name but it's not always. If you find the DLL in exporer, go to the properties right-click menu, and look in the Details tab the name is there under File Description.

You can use & for AND, | for OR and ! for NOT in the needs listing. To allow backwards compatibility , is treated as an alias for & (AND). If you combine several | and &, eg `NEEDS[Mod1|Mod2&!Mod3|Mod4]` this is treated as ( Mod1 OR Mod2 ) AND ( ( NOT Mod3 ) OR Mod4 ).  I won't be implementing brackets, it would make the parser far too complicated. There's always a way to represent what you want in this form, although it might need a few repeated terms, but I'm not sure I can truly see much of a use case for anything super complex.

In the below stuff, I've not put in the NEEDS section for clarity, however you can use it wherever you like.

## Operators available in patches - and their ordering

The general form of a patch for values is

`<Op><Name-With-Wildcards>(,<index>)?`

So breaking this down

- `<Op>` One of
 - nothing for [insert](#insert)
 - `@` for [edit](#edit----)
 - `+` or `$` for [copy](#copy----or-)
 - `-` or `!` for [delete](#delete----or--)
 - `%` for [edit-or-create](#edit-or-create---).
- `<Name-With-Wildcards>` :  The name of the value you'll be messing with. Wildcards are not always available for every Op. Wildcards include `?` for any character, and `*` for any number of chars. Note that ''only alphanumeric chars'' are allowed in value names for patches. If you have spaces or any other char, use a `?` to match it.
- `(,<index>)?` : Optional index. Again, not available with every option. Not that these indexes are ''with respect to all name matches'' not the total index in the node. I will support negative indexes for running backwards through the list soon. Also `*` is not yet supported.

The general form for nodes is:

`<Op><NodeType>([<NodeNameWithWildcards>])?(:HAS[<has block>])?(,<index-or-*>)?`

- `<Op>` : Operator, as above
- `<NodeType>` : typically MODULE or something like it. Wildcards not allowed (I can't imagine you'd need them)
- `([<NodeNameWithWildcards>])?` : This is a wildcard match for the `name = <name>` value within the node. Optional.
- `(:HAS[<has block>])?` : Optional has block. You can't (currently) use indexes with HAS. This has been described previously. If this is present then all matches will be matched, there's no indexing available currently.
- `(,<index-or-*>)?` : Index to select particular match zero based. 0 is the first node, -1 is last node. Values 'off the end' will match the end, so large positive matches the end and large negative matches the beginning. Again this matches ''against everything the wildcard selects in order''. `*` here will match everything.


### Insert

Insert is the default operator - it's what you get if you don't specify anything. You can now specify the index at which you'd like to insert your bit of config, otherwise it defaults to the end of the list

Example:

```
@PART[partName] 
{
    node_stack_atend = 0.0, 7.21461, 0.0, 0.0, 1.0, 0.0
    node_stack_atstart,0 =0.0, -7.21461, 0.0, 0.0, -1.0, 0.0  
    // Insert at the start
    MODULE,0 
    {
        name = ModuleSomething
    }
}
```

This will add an extra value called node_stack_atend at the end of the list, another value node_stack_atstart at the start, and insert a new module first in the list. 

Obviously wildcards, * indexes, and other stuff isn't available. Just the value or node name. If you do put in a wildcard it will end up in the output verbatim.

### Edit  - @

Edits the node or value in place. The order *will not change* (new since 2.0.9 ish). 

For nodes, all options are available to select the node, including indexes, * index, HAS, and wildcards in the name. If there are multiple matches and the index is not supplied, this will edit the first match. 

Example
```
@MMTEST[nodeEdit]
{
	// edit by name, will select the first one if multiple nodes with same name
	@MODULE[module2]	{ ... }
	// edit by index - will edit the second module (0 based indexing )
	@MODULE,1 { ... }
	// edit by name and index - will edit the second module with name=module2
	@MODULE[module2],1 { ... }
	// edit by wildcard and index - will edit the sixth module with name ending with 2. If there's no fifth, edit the last one defined
	@MODULE[*2],5 { ... }
	// edit by wildcard and index - edit the last module with three letter name starting with c
	@MODULE[c??],-1 { ... }
}
```

For values you additionally have the option of a replacement based on the existing node value, including standard arithmetic operators plus regular expressions for strings. Note: ''if no index is specified, only the first match will be edited''. Once ,* is implemented, then this behaviour should really change to all matches by default, so maybe would be a good idea for patch maintainers to move to using ,0 explicitly.

Example:

```
@PART[somePart1]
{
	// Unindexed - edits the first
	@name = somePart2
	// Arithmetic - add 5 and multiply by 20. Unindexed so edits the first one.
	@numeric += 5
	@numeric *= 20
	// Regexp expression. Replaces any instance of "tw" with "mo", so would turn "twotwo" into "moomoo"
        // Note also the index.
	@multiVal,1 ^= :tw:mo:
}
```

When using regexp replacements, the first character in the list is used as the separator. You can use whatever you like, but : is often a good choice. Obviously ensure that this isn't present in the regexp expression.

Please note that regex generation is often tricky and requires some experimentation to get right. It is assumed that if you want to use this feature you're well versed with how regexps work, including the various variants. Please refer to the .net [documentaton](http://msdn.microsoft.com/en-us/library/az24scfc(v=vs.110).aspx) and/or copious other documentation available on the Internet. I won't support questions about 'how do I do a regex to do xxx' on the list, you'll have to figure it out or look for help elsewhere.

Here's some useful regexps:

* `:$: Some Extra Stuff:` `$` matches the end of the string, so this is an easy way to add suffixes
* `:^: Preamble :` Similar to above. `^` matches the start of a string
* `:^.*$: Preamble $0 Some extra stuff:`   Combining the above. `^.*$` matches the entire input, and `$0` will stick it in in the output.

### Delete - ! or -

Delete a node or value. The order of everything afterwards will obviously drop back one step.

For nodes, all options are available to select the node, including indexes, * index, HAS, and wildcards in the name. If there are multiple matches and the index is not supplied, this will default to again to all nodes.

For values, again all options available. If no index is specified then all matching values are deleted - **This differs in behavior to edit**. I will likely change edit to doing this in the future.

Example:

```
@PART[nodeDelete]
{
	// Delete the first copy
	-MODULE[module2] {}
	// Indexed delete
	-MODULE,2 { }
	// Indexed delete from end
	-MODULE,-2 { }
	// Indexed delete off end
	-MODULE,9999 { }
	@MODULE[module1]
	{
		// Unindexed (remove all)
		-multiVal = dummy
		// Indexed
		-multiVal2,0 = dummy
		// Wildcard
		-num*ic = dummy
	}
}
```

Note that you still need to use { and } for nodes, and a dummy value for values. If you don't do this then the parser doesn't know what it's dealing with.


###Copy - + or $

Copy behaves identically to Edit, however rather than editing the node or value it copies it. It will always put the copy at the end of the list.

Note that for parts, you must always give a new name or it's a bit pointless:

```
+PART[myPartName]
{
    @name = myNewPartCopy
}
```

###Edit-or-Create - %

This will edit the value it it exists, otherwise it will create a new value as though this was an insert.

For existing nodes this will be identical to edit if the node exists, otherwise it will create an empty node, named as per the node name, and run it through the patch. Obviously because of the insert part, wildcards aren't allowed because the result wouldn't make sense. *this is not flagged as an error in the current build for nodes*;

For values this is identical to doing a delete and then an insert. Wildcards and indexes are not supported. The delete will delete all matches. This command has quite limited functionality, but it's there.

## Test cases / examples

Some automated test cases for this build [are here](https://github.com/sarbian/ModuleManager/tree/master/Tests). These also double as good examples of how it works.

## Variables ##
[Forum Post](http://forum.kerbalspaceprogram.com/threads/55219-Module-Manager-2-3-5-%28Sept-14%29-Loading-Speed-Fix?p=1416253&viewfull=1#post1416253)