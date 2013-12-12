namespace Ploeh.ZeroToNine

open System
open System.IO

module Program =

    let IncrementVersionsInFile rank file =
        let parse text =
            match Versioning.TryParse text with
            | None -> (text, None)
            | Some(pv) ->
                let newVersion = Versioning.IncrementVersion rank pv.Version
                let newText = pv.ToString newVersion
                let notification =
                    sprintf "Incremented %O %s from %O to %O"
                        (file |> FileSystem.GetRelativePath Environment.CurrentDirectory)
                        (pv.AttributeType.Name.Replace("Attribute", ""))
                        pv.Version
                        newVersion
                (newText, Some(notification))

        let writeAllLines file lines =
            File.WriteAllLines(file, lines, Text.Encoding.UTF8)

        let printn (s : string) = Console.WriteLine s

        let writeTo file parsedLines =
            parsedLines |> Array.map fst |> writeAllLines file
            parsedLines |> Array.choose snd |> Array.iter printn

        File.ReadAllLines file
        |> Array.map parse
        |> writeTo file

    let ListVersionsInFile file =
        let parse text =
            match Versioning.TryParse text with
            | None -> None
            | Some(pv) ->
                sprintf "%O %s %O"
                        (file |> FileSystem.GetRelativePath Environment.CurrentDirectory)
                        (pv.AttributeType.Name.Replace("Attribute", ""))
                        pv.Version
                |> Some

        let printn (s : string) = Console.WriteLine s

        File.ReadAllLines file
        |> Array.map parse
        |> Array.choose id 
        |> Array.iter printn

    let IncrementVersionsInAllAssemblyInfoFiles rank =
        Directory.GetFiles(
            Environment.CurrentDirectory,
            "AssemblyInfo.*",
            SearchOption.AllDirectories)
        |> Array.iter (IncrementVersionsInFile rank)

    let ListVersionsInAllAssemblyInfoFiles =
        Directory.GetFiles(
            Environment.CurrentDirectory,
            "AssemblyInfo.*",
            SearchOption.AllDirectories)
        |> Array.iter ListVersionsInFile

    [<EntryPoint>]
    let main argv = 
        match argv |> Args.Parse |> Seq.toList with
        | [Increment(rank)] -> IncrementVersionsInAllAssemblyInfoFiles rank
        | [ListVersions] -> ListVersionsInAllAssemblyInfoFiles
        | _ -> ()
        0 // return an integer exit code
