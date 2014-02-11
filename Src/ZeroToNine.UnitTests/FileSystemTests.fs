namespace Ploeh.ZeroToNine.UnitTests

open System
open Xunit
open Xunit.Extensions

module FileSystemTests =
    open Ploeh.ZeroToNine.FileSystem

    let private verify = Swensen.Unquote.Assertions.test
    
    [<Theory>]
    [<InlineData(@"C:\foo", @"C:\foo\bar", @".\bar")>]
    [<InlineData(@"C:\foo", @"C:\foo\baz", @".\baz")>]
    [<InlineData(@"C:\ploeh", @"C:\ploeh\fnaah", @".\fnaah")>]
    [<InlineData(@"C:\ploeh\", @"C:\ploeh\fnaah", @".\fnaah")>]
    [<InlineData(@"C:\ploeh", @"C:\foo\bar", @"C:\foo\bar")>]
    [<InlineData(@"C:\foo\baz", @"C:\foo\grault", @"C:\foo\grault")>]
    [<InlineData(@"C:\foo\bar", @"C:\foo\bar\baz\qux.corge", @".\baz\qux.corge")>]
    let GetRelativePathReturnsCorrectResult
        (workingDirectory : string)
        (path : string)
        (expected : string) =

        let actual = GetRelativePath workingDirectory path
        verify <@ expected = actual @>