namespace Ploeh.ZeroToNine.UnitTests

open Ploeh.ZeroToNine
open Ploeh.ZeroToNine.Versioning
open Xunit
open Xunit.Extensions

module ArgsTests =
    [<Theory>]
    [<InlineData("major",    Rank.Major)>]
    [<InlineData("minor",    Rank.Minor)>]
    [<InlineData("build",    Rank.Build)>]
    [<InlineData("revision", Rank.Revision)>]
    let ParseIncementReturnsCorrectResult(rank : string, expectedRank : Rank) =
        let actual = [| "-i"; rank |] |> Args.Parse
        Assert.Equal<Arg>([Increment(expectedRank)], actual |> Seq.toList)