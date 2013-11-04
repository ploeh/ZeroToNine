namespace Ploeh.ZeroToNine.UnitTests

open System
open Xunit
open Xunit.Extensions

module VersioningTests =
    open Ploeh.ZeroToNine.Versioning

    [<Theory>]
    [<InlineData("1.0.0.0", Rank.Major,    "2.0.0.0")>]
    [<InlineData("2.0.0.0", Rank.Major,    "3.0.0.0")>]
    [<InlineData("2.1.0.0", Rank.Major,    "3.0.0.0")>]
    [<InlineData("2.0.1.0", Rank.Major,    "3.0.0.0")>]
    [<InlineData("2.0.0.1", Rank.Major,    "3.0.0.0")>]
    [<InlineData("1.0.0.0", Rank.Minor,    "1.1.0.0")>]
    [<InlineData("2.0.0.0", Rank.Minor,    "2.1.0.0")>]
    [<InlineData("1.1.0.0", Rank.Minor,    "1.2.0.0")>]
    [<InlineData("1.0.1.0", Rank.Minor,    "1.1.0.0")>]
    [<InlineData("1.0.0.1", Rank.Minor,    "1.1.0.0")>]
    [<InlineData("1.0.0.0", Rank.Build,    "1.0.1.0")>]
    [<InlineData("1.0.1.0", Rank.Build,    "1.0.2.0")>]
    [<InlineData("2.0.0.0", Rank.Build,    "2.0.1.0")>]
    [<InlineData("1.1.0.0", Rank.Build,    "1.1.1.0")>]
    [<InlineData("1.0.0.1", Rank.Build,    "1.0.1.0")>]
    [<InlineData("1.0.0.0", Rank.Revision, "1.0.0.1")>]
    [<InlineData("1.0.0.1", Rank.Revision, "1.0.0.2")>]
    [<InlineData("2.0.0.0", Rank.Revision, "2.0.0.1")>]
    [<InlineData("1.1.0.0", Rank.Revision, "1.1.0.1")>]
    [<InlineData("1.0.1.0", Rank.Revision, "1.0.1.1")>]
    let IncrementVersionReturnsCorrectResult
        (version : string)
        (rank : Rank)
        (exp : string) =

        let v = Version.Parse version
        
        let actual = IncrementVersion rank v

        let expected = Version.Parse exp
        Assert.Equal(expected, actual)