# ZeroToNine

A tool for maintaining .NET Assembly versions across multiple files.

## Organization

*ZeroToNine* consists of two .NET assemblies:

- *Zero29* is a command-line utility.
- *ZeroToNine* is a library containing most of the implementation of *Zero29*.

While *Zero29* is the raison d'être for the *ZeroToNine* project, this organization enables other users to reuse the implemnentation logic without having to reference an executable assembly.