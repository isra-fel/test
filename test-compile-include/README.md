This solution simulates the scenario where 1 source file (type) is shared by 2 class libraries, one depending on another, by using `<Compile>` tag.

Unless the shared type is internal, there will be conflicts as there are two candidates when compiler resolves that type - one in the current library, one imported via the dependency library.

Though compiler knows which one to choose, it generates a warning [CS0436](https://docs.microsoft.com/en-us/dotnet/csharp/misc/cs0436). It can be resolved by:

- Marking the type as internal (if possible), then it would be internal to each project, hence no conflicts.
- Suppressing the warning, as the compiler knows the better choice.
- Giving the assembly reference an alias, and specify the alias before using namespaces. See https://stackoverflow.com/questions/3672920/two-different-dll-with-same-namespace

## A more complex scenario is...

... when a third project (for example a test project) references both libraries, who both made internal visible to the third project.

In that case, it would be an error instead of warning, as compiler does not know which one to favor. Besides, making the share type internal would not work. The "aliasing references" approach is the only option.
