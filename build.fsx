#r "paket:
nuget Fake.DotNet.Cli
nuget Fake.IO.FileSystem
nuget Fake.Core.Target //"
#load ".fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

Target.initEnvironment ()

Target.create "Clean" (fun _ ->
    !! "src/**/bin"
    ++ "src/**/obj"
    |> Shell.cleanDirs
)

Target.create "Build" (fun _ ->
    !! "src/**/*.*proj"
    |> Seq.iter (DotNet.build id)
)

Target.create "CopyLib" (fun _ ->
    [ @"lib/MessagePack.Altseed2/lib/Altseed2"
    ] |> Seq.iter (fun target ->
        Shell.copyDir target @"lib/Altseed2" (fun _ -> true)
    )
)

Target.create "PublishServer" (fun _ ->
    let projectName = "ACAC2020_15.Server"

    sprintf "src/%s/%s.csproj" projectName projectName
    |> DotNet.publish (fun p ->
      { p with
          Runtime = Some "linux-x64"
          Configuration = DotNet.BuildConfiguration.Release
          SelfContained = Some true
          MSBuildParams = {
            p.MSBuildParams with
              Properties =
                ("PublishSingleFile", "true")
                :: ("PublishTrimmed", "true")
                :: p.MSBuildParams.Properties
          }
          OutputPath = sprintf "publish/%s.linux-x64" projectName |> Some
      }
    )
)

Target.create "All" ignore

"Clean"
  ==> "Build"
  ==> "All"

Target.runOrDefault "All"
