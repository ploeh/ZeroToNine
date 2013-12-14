namespace Ploeh.ZeroToNine

open Ploeh.ZeroToNine.Versioning

type Arg =
    | Increment of Rank
    | ListVersions
    | ShowHelp
    | Unknown of string list

module Args =
    let Parse argv =
        match argv |> Seq.toList with
        | ["-l"] -> ListVersions
        | ["-i"; "major"] -> Increment(Rank.Major)
        | ["-i"; "minor"] -> Increment(Rank.Minor)
        | ["-i"; "build"] -> Increment(Rank.Build)
        | ["-i"; "patch"] -> Increment(Rank.Build)
        | ["-i"; "revision"] -> Increment(Rank.Revision)
        | ["-i"] -> Increment(Rank.Revision)
        | ["-?"] -> ShowHelp
        | ["-h"] -> ShowHelp
        | [] -> ShowHelp
        | x -> Unknown(x)
        |> Seq.singleton

