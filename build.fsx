#I @"packages/FAKE/tools/"
#I @"packages/Aardvark.Build/lib/net45"
#I @"packages/Mono.Cecil/lib/net45"
#I @"packages/Paket.Core/lib/net45"
#r @"System.Xml.Linq"
#r @"FakeLib.dll"
#r @"Aardvark.Build.dll"
#r @"Mono.Cecil.dll"
#r @"Paket.Core.dll"

#load @"bin/addSources.fsx"

open Fake
open System
open System.IO
open Aardvark.Build
open System.Text.RegularExpressions
open AdditionalSources
do Environment.CurrentDirectory <- __SOURCE_DIRECTORY__
let packageNameRx = Regex @"(?<name>[a-zA-Z_0-9\.]+?)\.(?<version>([0-9]+\.)*[0-9]+)\.nupkg"

Target "Install" (fun () ->
    AdditionalSources.paketDependencies.Install(false, false, false, true)
    AdditionalSources.installSources ()
)

Target "Restore" (fun () ->
    AdditionalSources.paketDependencies.Restore()
    AdditionalSources.installSources ()
)

Target "Update" (fun () ->
    AdditionalSources.paketDependencies.Update(false, false)
    AdditionalSources.installSources ()
)

Target "AddSource" (fun () ->
    let args = Environment.GetCommandLineArgs()
    let folders =
        if args.Length > 3 then
            Array.skip 3 args
        else
            failwith "no source folder given"

    AdditionalSources.addSources (Array.toList folders)
)

Target "RemoveSource" (fun () ->
    let args = Environment.GetCommandLineArgs()
    let folders =
        if args.Length > 3 then
            Array.skip 3 args
        else
            failwith "no source folder given"

    AdditionalSources.removeSources (Array.toList folders)
)


Target "Clean" (fun () ->
    DeleteDir (Path.Combine("bin", "Release"))
    DeleteDir (Path.Combine("bin", "Debug"))
)



Target "Compile" (fun () ->
    MSBuildRelease "bin/Release" "Build" ["src/GLSLang.sln"] |> ignore
)



Target "Default" (fun () -> ())


"Restore" ==> 
    "Compile" ==>
    "Default"


Target "CreatePackage" (fun () ->
    let releaseNotes = try Fake.Git.Information.getCurrentHash() |> Some with _ -> None
    if releaseNotes.IsNone then 
        //traceError "could not grab git status. Possible source: no git, not a git working copy"
        failwith "could not grab git status. Possible source: no git, not a git working copy"
    else 
        trace "git appears to work fine."
    
    let releaseNotes = releaseNotes.Value
    let branch = try Fake.Git.Information.getBranchName "." with e -> "master"

    let tag = Fake.Git.Information.getLastTag()
    AdditionalSources.paketDependencies.Pack("bin", version = tag, releaseNotes = releaseNotes)
)

Target "InjectNativeDependencies" (fun () ->

    if Directory.Exists "bin/Debug" then
        File.Copy(@"packages/Aardvark.Build/lib/net45/Aardvark.Build.dll", @"bin/Debug/Aardvark.Build.dll", true)
    if Directory.Exists "bin/Release" then
        File.Copy(@"packages/Aardvark.Build/lib/net45/Aardvark.Build.dll", @"bin/Release/Aardvark.Build.dll", true)

    let dirs = Directory.GetDirectories "libs/Native"
    for d in dirs do
        let n = Path.GetFileName d
        let d = d |> Path.GetFullPath
        let paths = [
            Path.Combine("bin/Release", n + ".dll") |> Path.GetFullPath
            Path.Combine("bin/Release", n + ".exe") |> Path.GetFullPath
            Path.Combine("bin/Debug", n + ".dll") |> Path.GetFullPath
            Path.Combine("bin/Debug", n + ".exe") |> Path.GetFullPath
        ]

        let wd = Environment.CurrentDirectory
        try
        
            for p in paths do
                if File.Exists p then
                    Environment.CurrentDirectory <- Path.GetDirectoryName p
                    let ass = Mono.Cecil.AssemblyDefinition.ReadAssembly(p)
                    AssemblyInjector.addResources d ass
                    ass.Write p
                    tracefn "injected native stuff in %A" p
        finally
            Environment.CurrentDirectory <- wd

        ()

    ()
)



Target "Push" (fun () ->
    let packages = !!"bin/*.nupkg"
    let packageNameRx = Regex @"(?<name>[a-zA-Z_0-9\.]+?)\.(?<version>([0-9]+\.)*[0-9]+)\.nupkg"
    let tag = Fake.Git.Information.getLastTag()

    let myPackages = 
        packages 
            |> Seq.choose (fun p ->
                let m = packageNameRx.Match (Path.GetFileName p)
                if m.Success then 
                    Some(m.Groups.["name"].Value)
                else
                    None
            )
            |> Set.ofSeq

    try
        for id in myPackages do
            let source = sprintf "bin/%s.%s.nupkg" id tag
            let target = sprintf @"\\hobel.ra1.vrvis.lan\NuGet\%s.%s.nupkg" id tag
            File.Copy(source, target, true)
    with e ->
        traceError (string e)
)

let deploy (url : string) (keyName : Option<string>) =

    let packages = !!"bin/*.nupkg"
    let packageNameRx = Regex @"(?<name>[a-zA-Z_0-9\.]+?)\.(?<version>([0-9]+\.)*[0-9]+)\.nupkg"

    let myPackages = 
        packages 
            |> Seq.choose (fun p ->
                let m = packageNameRx.Match (Path.GetFileName p)
                if m.Success then 
                    Some(m.Groups.["name"].Value)
                else
                    None
            )
            |> Set.ofSeq

   
    let accessKey =
        match keyName with
         | Some keyName -> 
            let accessKeyPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ssh", keyName)
            if File.Exists accessKeyPath then 
                let r = Some (File.ReadAllText accessKeyPath)
                tracefn "key: %A" r.Value
                r
            else None
         | None -> None


    let branch = Fake.Git.Information.getBranchName "."
    let releaseNotes = Fake.Git.Information.getCurrentHash()
    let dep =
        if branch <> "master" then
            tracefn "are you really sure you want do deploy a non-master branch? (Y/N)"
            let l = Console.ReadLine().Trim().ToLower()
            l = "y"
        else true

    if dep then
        let tag = Fake.Git.Information.getLastTag()
        match accessKey with
            | Some accessKey ->
                try
                    for id in myPackages do
                        let packageName = sprintf "bin/%s.%s.nupkg" id tag
                        tracefn "pushing: %s" packageName
                        Paket.Dependencies.Push(packageName, apiKey = accessKey, url = url)
                with e ->
                    traceError (string e)
            | None ->
                traceError (sprintf "Could not find nuget access key")

Target "MyGetDeploy" (fun () -> 
    deploy "https://vrvis.myget.org/F/aardvark/api/v2/" (Some "myget.key") 
)

Target "InternalDeploy" DoNothing

"CreatePackage" ==> "MyGetDeploy"
"CreatePackage" ==> "Push"

"Push" ==> "InternalDeploy"
"MyGetDeploy" ==> "InternalDeploy"

"Compile" ==> "InjectNativeDependencies" ==> "CreatePackage"

// start build
RunTargetOrDefault "Default"

