namespace Ploeh.ZeroToNine

open Ploeh.ZeroToNine.Versioning

type Arg =
    | Increment of Rank

module Args =
    let Parse argv =
        match argv |> Seq.toList with
        | [_; "major"] -> Increment(Rank.Major)
        | [_; "minor"] -> Increment(Rank.Minor)
        | [_; "build"] -> Increment(Rank.Build)
        | _ -> Increment(Rank.Revision)
        |> Seq.singleton

