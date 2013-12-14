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
    [<InlineData("patch",    Rank.Build)>]
    [<InlineData("revision", Rank.Revision)>]
    let ParseIncementReturnsCorrectResult(rank : string, expectedRank : Rank) =
        let actual = [| "-i"; rank |] |> Args.Parse
        Assert.Equal<Arg>([Increment(expectedRank)], actual |> Seq.toList)

    [<Fact>]
    let ParseIncrementWithoutRankReturnsCorrectResult() =
        let actual = [| "-i" |] |> Args.Parse
        Assert.Equal<Arg>([Increment(Rank.Revision)], actual |> Seq.toList)

    [<Fact>]
    let ParseListVersion() =
        let actual = [| "-l" |] |> Args.Parse
        Assert.Equal<Arg>([ListVersions], actual |> Seq.toList)

    [<Theory>]
    [<InlineData("-?")>]
    [<InlineData("-h")>]
    let ParseHelpArgsReturnsShowHelp(switch : string) =
        let actual = [| switch |] |> Args.Parse
        Assert.Equal<Arg>([ShowHelp], actual |> Seq.toList)

    [<Fact>]
    let ParseEmptyArgsReturnsShowHelp() =
        let actual = [| |] |> Args.Parse
        Assert.Equal<Arg>([ShowHelp], actual |> Seq.toList)
