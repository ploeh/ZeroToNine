namespace Ploeh.ZeroToNine

open System

module Versioning =
    type Rank =
        | Major = 3
        | Minor = 2
        | Build = 1
        | Revision = 0

    let IncrementVersion rank (version : Version) =
        match rank with
        | Rank.Major ->
            Version(version.Major + 1, 0, 0, 0)
        | Rank.Minor ->
            Version(version.Major, version.Minor + 1, 0, 0)
        | Rank.Build ->
            Version(version.Major, version.Minor, version.Build + 1, 0)
        | _ ->
            Version(version.Major, version.Minor, version.Build, version.Revision + 1)

