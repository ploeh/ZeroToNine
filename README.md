# ZeroToNine

A tool for maintaining .NET Assembly versions across multiple source files.

## Organization

*ZeroToNine* consists of two .NET assemblies:

- *Zero29* is a command-line utility.
- *ZeroToNine* is a library containing most of the implementation of *Zero29*.

While *Zero29* is the raison d'être for the *ZeroToNine* project, this organization enables other users to reuse the implementation logic without having to reference an executable assembly.

### NuGet packages

Signed versions of both *Zero29* and *ZeroToNine* are available via NuGet:

- [Zero29](https://www.nuget.org/packages/Zero29)
- [ZeroToNine](https://www.nuget.org/packages/ZeroToNine) 

## Command-line usage

*Zero29* is a command-line utility:

```
Zero29 <command> [<args>]
```

### Commands

```
-i <major|minor|build|patch|revision>
```

Increments the specified component of each Assembly Version and Assembly File Version Attributes within all `AssemblyInfo.*` files beneath the current working directory.

`build` and `patch` are synonyms. The [Version](http://msdn.microsoft.com/en-us/library/system.version.aspx) type refers to the third component as the *build*, whereas [Semantic Versioning](http://semver.org) terms it *patch*.

#### Examples

```
Zero29 -i minor
```

Increments the minor versions in all `AssemblyInfo.*` files; e.g. from *1.0.1.0* to *1.1.0.0*.

```
Zero29 -i patch
```

Increments the patch version in all `AssemblyInfo.*` files; e.g. from *1.0.1.0* to *1.0.2.0*.
