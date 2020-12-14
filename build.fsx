#r "paket:
nuget Fake.DotNet.Cli
nuget Fake.IO.FileSystem
nuget Fake.Core.Target
nuget FAKE.IO.Zip //"
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

let publishProject projectName ext runtime =
  let outputPath = sprintf "publish/%s.%s" projectName runtime

  sprintf "src/%s/%s.%s" projectName projectName ext
  |> DotNet.publish (fun p ->
    { p with
        Runtime = Some runtime
        Configuration = DotNet.BuildConfiguration.Release
        SelfContained = Some true
        MSBuildParams = {
          p.MSBuildParams with
            Properties =
              ("PublishSingleFile", "true")
              :: ("PublishTrimmed", "true")
              :: p.MSBuildParams.Properties
        }
        OutputPath = outputPath |> Some
    }
  )

  "LICENSE_publish.txt"
  |> Shell.copyFile (sprintf "%s/LICENSE.txt" outputPath)

  !! (sprintf "%s/**" outputPath)
  |> Zip.zip "publish" (sprintf "%s.zip" outputPath)


let publishCsproj projectName runtime = publishProject projectName "csproj" runtime


Target.create "PublishServer" (fun _ ->
    let projectName = "ACAC2020_15.Server"
    let runtime = "linux-x64"
    publishCsproj projectName runtime
)

Target.create "PublishClient" (fun _ ->
    let projectName = "ACAC2020_15.Client"
    let runtimes = [ "win-x64"; "osx-x64" ]

    runtimes |> Seq.iter (publishCsproj projectName)
)

Target.create "All" ignore

"Clean"
  ==> "Build"
  ==> "All"

Target.runOrDefault "All"
