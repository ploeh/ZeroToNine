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

### Versioning

ZeroToNine follows [Semantic Versioning 2.0.0](http://semver.org/spec/v2.0.0.html). 

## Command-line usage

*Zero29* is a command-line utility:

```
Zero29 <command> [<args>]
```

### Commands

The following commands are available:

- Increment
- List
- Help

#### Increment 

```
-i <major|minor|build|patch|revision>
```

Increments the specified component of each Assembly Version and Assembly File Version Attribute within all `AssemblyInfo.*` files beneath the current working directory.

`build` and `patch` are synonyms. The [Version](http://msdn.microsoft.com/en-us/library/system.version.aspx) type refers to the third component as the *build*, whereas [Semantic Versioning](http://semver.org) terms it *patch*.

##### Examples

```
Zero29 -i minor
```

Increments the minor versions in all `AssemblyInfo.*` files; e.g. from *1.0.1.0* to *1.1.0.0*.

```
Zero29 -i patch
```

Increments the patch version in all `AssemblyInfo.*` files; e.g. from *1.0.1.0* to *1.0.2.0*.

#### Assign

```
-a <version>
```

Assigns the specified `version` value to all Assembly Version and Assembly File Version attributes within all `AssemblyInfo.*` files beneath the current working directory.

This can be valuable in different scenarios; for example

- To initiate a completely new code folder with a particular version. For example, by default Visual Studio assigns the version 1.0.0.0 to new C# library projects. In this case, the *assign* operation can be used to set all version information to a common version, e.g. 0.1.0.
- When adding one or more new projects to a code base, the new projects may not match the existing versions in established projects. The *assign* operation can be used to assign all version values to the (existing) version number.

##### Examples

```
Zero29 -a 1.3.2
```

Assigns the version number 1.3.2 to all version attributes in all `AssemblyInfo.*` files. 

#### Assign version component

```
-a <major|minor|build|patch|revision> <version number>
```

Assigns a particular version number to a specified component (major, minor, etc.) of the existing version of all Assembly Version and Assembly File Version attributes within all `AssemblyInfo.*` files beneath the current working directory.

This allows for explicit control over the individual parts of a version and enables scenarios whereby a development team may want explicit control over certain version parts such as major and minor, but allow a Continuous Integration (CI) server to set the build part (for example, to be able to link a deployed version of an application with the CI server build).

##### Examples

```
Zero29 -a build 10
```

Assigns the number 10 to the build part of all existing version attributes in all `AssemblyInfo.*` files. 


#### List

```
-l
```

Lists the versions of each Assembly Version and Assembly File Version Attribute within all `AssemblyInfo.*` files beneath the current working directory.

##### Examples

```
Zero29 -l
```

Lists the version information found in the appropriate source files beneath the current working directory.

###### Sample output

```
./Foo/AssemblyInfo.fs AssemblyVersion 1.1.0.0
./Foo/AssemblyInfo.fs AssemblyFileVersion 1.1.0.0
./Bar/AssemblyInfo.cs AssemblyVersion 2.0.3.0
./Bar/AssemblyInfo.cs AssemblyFileVersion 2.0.3.0
```

###### Listing unique versions

In bash, you can pipe the output of the *list* operation to other bash commands in order to get a list of unique versions:

```
Zero29.exe -l | awk '{ print $3; }' | sort | uniq
```

###### Sample output

Given the raw output from the previous example, the output of this command would be:

```
1.1.0.0
2.0.3.0
```

#### Help

```
-h | -?
```

Displays the help about available commands.

##### Examples

```
Zero29 -h
```
