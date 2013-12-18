namespace Ploeh.ZeroToNine

open System
open System.Text.RegularExpressions

module Versioning =
    type Rank =
        | Major = 3
        | Minor = 2
        | Build = 1
        | Revision = 0

    type ParsedVersion = {
        Version : Version
        AttributeType : Type
        ToString : Version -> string }

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

    let private regx = Regex("""(^\s*[\[|<]<?\s*[a|A]ssembly\s*:\s*Assembly(File)?Version\s*\(\s*)"(\d+\.\d+\.\d+\.\d+)"(\s*\)\s*>?[\]|>]\s*$)""", RegexOptions.Compiled)
    
    let TryParse text =
        let m = regx.Match text
        if m.Success then
            let attributeType =
                match m.Groups.[2].Value.ToUpperInvariant() with
                | "FILE" -> typeof<System.Reflection.AssemblyFileVersionAttribute>
                | _ -> typeof<System.Reflection.AssemblyVersionAttribute>
            {
                Version =  Version(m.Groups.[3].Value)
                AttributeType = attributeType
                ToString = (fun v -> regx.Replace(text, sprintf """$1"%O"$4""" v))
             }
             |> Some
        else
            None

    let IncrementAssemblyAttribute rank text =
        match TryParse text with
        | None -> text
        | Some(pv) -> pv.Version |> IncrementVersion rank |> pv.ToString