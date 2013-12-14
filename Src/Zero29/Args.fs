namespace Ploeh.ZeroToNine

open Ploeh.ZeroToNine.Versioning

type Arg =
    | Increment of Rank
    | ListVersions 
    | ShowHelp

module Args =
    let Parse argv =
        match argv |> Seq.toList with
        | ["-?"] -> ShowHelp
        | ["-h"] -> ShowHelp
        | ["-l"] -> ListVersions
        | [_; "major"] -> Increment(Rank.Major)
        | [_; "minor"] -> Increment(Rank.Minor)
        | [_; "build"] -> Increment(Rank.Build)
        | [_; "patch"] -> Increment(Rank.Build)
        | [_; _] -> Increment(Rank.Revision)
        | [_] -> Increment(Rank.Revision)
        | _ -> ShowHelp
        |> Seq.singleton

