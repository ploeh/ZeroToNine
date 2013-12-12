namespace Ploeh.ZeroToNine

open System
open System.IO

module Program =
    
    let private printn (s : string) = Console.WriteLine s

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

        let writeTo file parsedLines =
            parsedLines |> Array.map fst |> writeAllLines file
            parsedLines |> Array.choose snd |> Array.iter printn

        File.ReadAllLines file
        |> Array.map parse
        |> writeTo file

    let ListVersionsInFile file =
        let formatVersion (pv : Versioning.ParsedVersion) =
            sprintf "%O %s %O"
                (file |> FileSystem.GetRelativePath Environment.CurrentDirectory)
                (pv.AttributeType.Name.Replace("Attribute", ""))
                pv.Version

        let parse text =
            Versioning.TryParse text
            |> Option.map formatVersion                            

        File.ReadAllLines file
        |> Array.choose parse
        |> Array.iter printn
    
    let DoInAllAssemblyInfoFiles action =
        Directory.GetFiles(
            Environment.CurrentDirectory,
            "AssemblyInfo.*",
            SearchOption.AllDirectories)
        |> Array.iter action

    [<EntryPoint>]
    let main argv = 
        match argv |> Args.Parse |> Seq.toList with
        | [Increment(rank)] -> IncrementVersionsInFile rank |> DoInAllAssemblyInfoFiles
        | [ListVersions] -> ListVersionsInFile |> DoInAllAssemblyInfoFiles
        | _ -> ()
        0 // return an integer exit code
