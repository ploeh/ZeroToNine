namespace Ploeh.ZeroToNine

open System.IO

module FileSystem =
    let GetRelativePath (workingDirectory : string) (path : string) =
        match path.IndexOf workingDirectory with
        | 0 ->
            let wdLength =
                workingDirectory.TrimEnd(Path.DirectorySeparatorChar).Length
            "." + path.Substring wdLength
        | _ -> path

