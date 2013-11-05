namespace Ploeh.ZeroToNine

open System
open System.IO

module Program =

    let IncrementVersionsInFile rank file =
        let writeAllLines file lines = File.WriteAllLines(file, lines)

        File.ReadAllLines file
        |> Array.map (Versioning.IncrementAssemblyAttribute rank)
        |> writeAllLines file

    let IncrementVersionsInAllAssemblyInfoFiles rank =
        Directory.GetFiles(
            Environment.CurrentDirectory,
            "AssemblyInfo.*",
            SearchOption.AllDirectories)
        |> Array.iter (IncrementVersionsInFile rank)

    [<EntryPoint>]
    let main argv = 
        match argv |> Args.Parse |> Seq.toList with
        | [Increment(rank)] -> IncrementVersionsInAllAssemblyInfoFiles rank
        | _ -> ()
        0 // return an integer exit code
