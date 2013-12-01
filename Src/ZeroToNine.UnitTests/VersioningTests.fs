namespace Ploeh.ZeroToNine.UnitTests

open System
open Xunit
open Xunit.Extensions

module VersioningTests =
    open Ploeh.ZeroToNine.Versioning

    [<Theory>]
    [<InlineData("1.0.0.0", Rank.Major,    "2.0.0.0")>]
    [<InlineData("2.0.0.0", Rank.Major,    "3.0.0.0")>]
    [<InlineData("2.1.0.0", Rank.Major,    "3.0.0.0")>]
    [<InlineData("2.0.1.0", Rank.Major,    "3.0.0.0")>]
    [<InlineData("2.0.0.1", Rank.Major,    "3.0.0.0")>]
    [<InlineData("1.0.0.0", Rank.Minor,    "1.1.0.0")>]
    [<InlineData("2.0.0.0", Rank.Minor,    "2.1.0.0")>]
    [<InlineData("1.1.0.0", Rank.Minor,    "1.2.0.0")>]
    [<InlineData("1.0.1.0", Rank.Minor,    "1.1.0.0")>]
    [<InlineData("1.0.0.1", Rank.Minor,    "1.1.0.0")>]
    [<InlineData("1.0.0.0", Rank.Build,    "1.0.1.0")>]
    [<InlineData("1.0.1.0", Rank.Build,    "1.0.2.0")>]
    [<InlineData("2.0.0.0", Rank.Build,    "2.0.1.0")>]
    [<InlineData("1.1.0.0", Rank.Build,    "1.1.1.0")>]
    [<InlineData("1.0.0.1", Rank.Build,    "1.0.1.0")>]
    [<InlineData("1.0.0.0", Rank.Revision, "1.0.0.1")>]
    [<InlineData("1.0.0.1", Rank.Revision, "1.0.0.2")>]
    [<InlineData("2.0.0.0", Rank.Revision, "2.0.0.1")>]
    [<InlineData("1.1.0.0", Rank.Revision, "1.1.0.1")>]
    [<InlineData("1.0.1.0", Rank.Revision, "1.0.1.1")>]
    let IncrementVersionReturnsCorrectResult
        (version : string)
        (rank : Rank)
        (exp : string) =

        let v = Version.Parse version
        
        let actual = IncrementVersion rank v

        let expected = Version.Parse exp
        Assert.Equal(expected, actual)

    [<Theory>]
    [<InlineData(Rank.Major, "", "")>]
    [<InlineData(Rank.Major, "ploeh", "ploeh")>]
    [<InlineData(Rank.Major, """[assembly:AssemblyVersion("1.0.0.0")]""", """[assembly:AssemblyVersion("2.0.0.0")]""")>]
    [<InlineData(Rank.Major, """[assembly: AssemblyVersion("1.0.0.0")]""", """[assembly: AssemblyVersion("2.0.0.0")]""")>]
    [<InlineData(Rank.Minor, """[assembly:  AssemblyVersion("1.0.0.0")]""", """[assembly:  AssemblyVersion("1.1.0.0")]""")>]
    [<InlineData(Rank.Major, """  [assembly:AssemblyVersion("2.0.0.0")]""", """  [assembly:AssemblyVersion("3.0.0.0")]""")>]
    [<InlineData(Rank.Build, """[  assembly:AssemblyVersion("2.0.0.0")]""", """[  assembly:AssemblyVersion("2.0.1.0")]""")>]
    [<InlineData(Rank.Minor, """[assembly  :AssemblyVersion("2.2.0.0")]""", """[assembly  :AssemblyVersion("2.3.0.0")]""")>]
    [<InlineData(Rank.Major, """[assembly: AssemblyVersion  ("2.2.0.0")]""", """[assembly: AssemblyVersion  ("3.0.0.0")]""")>]
    [<InlineData(Rank.Build, """[assembly: AssemblyVersion(  "1.2.0.0")]""", """[assembly: AssemblyVersion(  "1.2.1.0")]""")>]
    [<InlineData(Rank.Revision, """[assembly: AssemblyVersion("1.2.0.0"  )]""", """[assembly: AssemblyVersion("1.2.0.1"  )]""")>]
    [<InlineData(Rank.Major, """[assembly: AssemblyVersion("1.2.0.0")  ]""", """[assembly: AssemblyVersion("2.0.0.0")  ]""")>]
    [<InlineData(Rank.Major, """[assembly: AssemblyVersion("1.2.0.0")]  """, """[assembly: AssemblyVersion("2.0.0.0")]  """)>]
    [<InlineData(Rank.Major, """[assembly: AssemblyFileVersion("1.0.0.0")]""", """[assembly: AssemblyFileVersion("2.0.0.0")]""")>]
    [<InlineData(Rank.Minor, """[<assembly: AssemblyVersion("3.12.1.0")>]""", """[<assembly: AssemblyVersion("3.13.0.0")>]""")>]
    let IncrementInTextReturnsCorrectResult
        (rank: Rank)
        (text : string)
        (expected : string) =
        
        let actual = IncrementAssemblyAttribute rank text
        Assert.Equal<string>(expected, actual)
    
    [<Theory>]
    [<InlineData("")>]
    [<InlineData("ploeh")>]
    let TryParseNoVersionInformationReturnsNone(text : string) =
        let actual = TryParse text
        Assert.Equal(None, actual)

    [<Theory>]
    [<InlineData("""[assembly:AssemblyVersion("1.0.0.0")]""", "1.0.0.0")>]
    [<InlineData("""[assembly: AssemblyVersion("1.0.0.1")]""", "1.0.0.1")>]
    [<InlineData("""[assembly:  AssemblyVersion("1.0.0.0")]""", "1.0.0.0")>]
    [<InlineData("""  [assembly:AssemblyVersion("2.0.0.0")]""", "2.0.0.0")>]
    [<InlineData("""[  assembly:AssemblyVersion("2.0.0.0")]""", "2.0.0.0")>]
    [<InlineData("""[assembly  :AssemblyVersion("2.2.0.0")]""", "2.2.0.0")>]
    [<InlineData("""[assembly: AssemblyVersion  ("2.2.0.0")]""", "2.2.0.0")>]
    [<InlineData("""[assembly: AssemblyVersion(  "1.2.0.0")]""", "1.2.0.0")>]
    [<InlineData("""[assembly: AssemblyVersion("1.2.0.0"  )]""", "1.2.0.0")>]
    [<InlineData("""[assembly: AssemblyVersion("1.2.0.0")  ]""", "1.2.0.0")>]
    [<InlineData("""[assembly: AssemblyVersion("1.2.0.0")]  """, "1.2.0.0")>]
    [<InlineData("""[assembly: AssemblyFileVersion("1.0.0.0")]""", "1.0.0.0")>]
    [<InlineData("""[<assembly: AssemblyVersion("3.12.1.0")>]""", "3.12.1.0")>]
    let TryParseVersionInformationReturnsCorrectData
        (text : string)
        (expectedS : string) =

        let actual = TryParse text

        let expected = Version(expectedS)
        Assert.True actual.IsSome
        Assert.Equal(expected, actual.Value.Version)

    [<Theory>]
    [<InlineData("""[assembly:AssemblyVersion("1.0.0.0")]""", """[assembly:AssemblyVersion("2.0.0.0")]""", "2.0.0.0")>]
    [<InlineData("""[assembly: AssemblyVersion("1.0.0.0")]""", """[assembly: AssemblyVersion("2.0.0.0")]""", "2.0.0.0")>]
    [<InlineData("""[assembly:  AssemblyVersion("1.0.0.0")]""", """[assembly:  AssemblyVersion("1.1.0.0")]""", "1.1.0.0")>]
    [<InlineData("""  [assembly:AssemblyVersion("2.0.0.0")]""", """  [assembly:AssemblyVersion("3.0.0.0")]""", "3.0.0.0")>]
    [<InlineData("""[  assembly:AssemblyVersion("2.0.0.0")]""", """[  assembly:AssemblyVersion("2.0.1.0")]""", "2.0.1.0")>]
    [<InlineData("""[assembly  :AssemblyVersion("2.2.0.0")]""", """[assembly  :AssemblyVersion("4.6.8.0")]""", "4.6.8.0")>]
    [<InlineData("""[assembly: AssemblyVersion  ("2.2.0.0")]""", """[assembly: AssemblyVersion  ("3.0.0.0")]""", "3.0.0.0")>]
    [<InlineData("""[assembly: AssemblyVersion(  "1.2.0.0")]""", """[assembly: AssemblyVersion(  "1.2.1.0")]""", "1.2.1.0")>]
    [<InlineData("""[assembly: AssemblyVersion("1.2.0.0"  )]""", """[assembly: AssemblyVersion("1.2.0.1"  )]""", "1.2.0.1")>]
    [<InlineData("""[assembly: AssemblyVersion("1.2.0.0")  ]""", """[assembly: AssemblyVersion("2.0.0.0")  ]""", "2.0.0.0")>]
    [<InlineData("""[assembly: AssemblyVersion("1.2.0.0")]  """, """[assembly: AssemblyVersion("2.0.0.0")]  """, "2.0.0.0")>]
    [<InlineData("""[assembly: AssemblyFileVersion("1.0.0.0")]""", """[assembly: AssemblyFileVersion("2.0.0.0")]""", "2.0.0.0")>]
    [<InlineData("""[<assembly: AssemblyVersion("3.12.1.0")>]""", """[<assembly: AssemblyVersion("3.13.0.0")>]""", "3.13.0.0")>]
    let TryParseReturnsCorrectToStringFunction
        (text : string)
        (expected : string)
        (newVersion : string) =
        let actual = (TryParse text).Value.ToString (Version(newVersion))
        Assert.Equal<string>(expected, actual)

    [<Theory>]
    [<InlineData("""[assembly:AssemblyVersion("1.0.0.0")]""", typeof<System.Reflection.AssemblyVersionAttribute>)>]
    [<InlineData("""[assembly: AssemblyVersion("1.0.0.1")]""", typeof<System.Reflection.AssemblyVersionAttribute>)>]
    [<InlineData("""[assembly:  AssemblyVersion("1.0.0.0")]""", typeof<System.Reflection.AssemblyVersionAttribute>)>]
    [<InlineData("""  [assembly:AssemblyVersion("2.0.0.0")]""", typeof<System.Reflection.AssemblyVersionAttribute>)>]
    [<InlineData("""[  assembly:AssemblyVersion("2.0.0.0")]""", typeof<System.Reflection.AssemblyVersionAttribute>)>]
    [<InlineData("""[assembly  :AssemblyVersion("2.2.0.0")]""", typeof<System.Reflection.AssemblyVersionAttribute>)>]
    [<InlineData("""[assembly: AssemblyVersion  ("2.2.0.0")]""", typeof<System.Reflection.AssemblyVersionAttribute>)>]
    [<InlineData("""[assembly: AssemblyVersion(  "1.2.0.0")]""", typeof<System.Reflection.AssemblyVersionAttribute>)>]
    [<InlineData("""[assembly: AssemblyFileVersion("1.2.0.0"  )]""", typeof<System.Reflection.AssemblyFileVersionAttribute>)>]
    [<InlineData("""[assembly: AssemblyFileVersion("1.2.0.0")  ]""", typeof<System.Reflection.AssemblyFileVersionAttribute>)>]
    [<InlineData("""[assembly: AssemblyFileVersion("1.2.0.0")]  """, typeof<System.Reflection.AssemblyFileVersionAttribute>)>]
    [<InlineData("""[assembly: AssemblyFileVersion("1.0.0.0")]""", typeof<System.Reflection.AssemblyFileVersionAttribute>)>]
    [<InlineData("""[<assembly: AssemblyFileVersion("3.12.1.0")>]""", typeof<System.Reflection.AssemblyFileVersionAttribute>)>]
    let TryParseReturnsCorrectAttributeType
        (text : string)
        (expected : Type) =

        let actual = (TryParse text).Value.AttributeType
        Assert.Equal(expected, actual)