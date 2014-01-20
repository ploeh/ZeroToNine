﻿namespace Ploeh.ZeroToNine

open System
open System.IO

module Program =
    
    let private printn (s : string) = Console.WriteLine s

    let IncrementVersion rank pv =
        let newVersion = Versioning.IncrementVersion rank pv
        let notificationf = sprintf "Incremented %O %s from %O to %O"
        (newVersion, notificationf)

    let AssignVersion version pv =
        let newVersion = Version version
        let notificationf = sprintf "Assigned %O %s from %O to %O"
        (newVersion, notificationf)

    let UpdateVersionsInFile updateAction file =
        let update (pv:Versioning.ParsedVersion) =
            let (newVersion, notificationf) = updateAction pv.Version
            let newText = pv.ToString newVersion
            let notification =
                notificationf
                    (file |> FileSystem.GetRelativePath Environment.CurrentDirectory)
                    (pv.AttributeType.Name.Replace("Attribute", ""))
                    pv.Version
                    newVersion
            (newText, Some(notification))

        let parse text =
            Versioning.TryParse text
            |> Option.map update
            |> Option.fold (fun s t -> t) (text, None)

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

    let private Description =
        [
            "Zero29"
            "A tool for maintaining .NET Assembly versions across multiple source files."
            "Operates on all AssemblyInfo.* files beneath the current working directory."
        ]

    let private EmptyLine = [ "" ]

    let private display messages =
        messages
        |> Seq.collect id
        |> Seq.iter printn

    let ShowUsage() =
        [
            Description
            EmptyLine
            Args.Usage
        ]
        |> display

    let PrintUnrecognizedArgs args =
        let unrecognizedMessage =
            [
                "Sorry but I could not recognize any command"
                sprintf "  in %A" args
            ]
        [
            Description
            EmptyLine
            unrecognizedMessage
            EmptyLine
            Args.Usage
        ]
        |> display

    [<EntryPoint>]
    let main argv = 
        match argv |> Args.Parse |> Seq.toList with
        | [Increment(rank)] -> IncrementVersion rank |> UpdateVersionsInFile |> DoInAllAssemblyInfoFiles
        | [Assign(version)] -> AssignVersion version |> UpdateVersionsInFile |> DoInAllAssemblyInfoFiles
        | [ListVersions] -> ListVersionsInFile |> DoInAllAssemblyInfoFiles
        | [ShowHelp] -> ShowUsage()
        | [Unknown(args)] -> PrintUnrecognizedArgs args
        | _ -> ()
        0 // return an integer exit code
