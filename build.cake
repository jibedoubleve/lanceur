
///////////////////////////////////////////////////////////////////////////////
// TOOLS / ADDINS
///////////////////////////////////////////////////////////////////////////////
#tool vswhere
#tool GitVersion.CommandLine
#tool xunit.runner.console

#addin Cake.Figlet
#addin Cake.Powershell

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////
var repoName = "Lanceur";

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var verbosity = Argument("verbosity", Verbosity.Minimal);

///////////////////////////////////////////////////////////////////////////////
// PREPARATION
///////////////////////////////////////////////////////////////////////////////

//Files
var solution   = "./src/Probel.Lanceur.sln";
var publishDir = "./Publish/";
var inno_setup = "./build/setup.iss";

GitVersion gitVersion = GitVersion(new GitVersionSettings 
{ 
    OutputType = GitVersionOutput.Json,
    UpdateAssemblyInfo  = true,
    UpdateAssemblyInfoFilePath  = "./src/Version.cs",
});
var branchName = gitVersion.BranchName;

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////
Setup(ctx =>
{
    if(!IsRunningOnWindows())
    {
        throw new NotImplementedException($"{repoName} should only run on Windows");
    }
    
    Information(Figlet($"Probel   {repoName}"));

    Information("Configuration          : {0}", configuration);
    Information("Branch                 : {0}", branchName);
    Information("Informational      Version: {0}", gitVersion.InformationalVersion);
    Information("SemVer             Version: {0}", gitVersion.SemVer);
    Information("AssemblySemVer     Version: {0}", gitVersion.AssemblySemVer);
    Information("AssemblySemFileVer Version: {0}", gitVersion.AssemblySemFileVer);
    Information("MajorMinorPatch    Version: {0}", gitVersion.MajorMinorPatch);
    Information("NuGet              Version: {0}", gitVersion.NuGetVersion);  
    Information("Configuration             : {0}", configuration);
});
///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////
Task("Clean")
    .Does(()=> 
    {
        var dirToDelete = GetDirectories("./**/obj")
                            .Concat(GetDirectories("./**/bin"))
                            .Concat(GetDirectories("./**/Publish"));
        DeleteDirectories(dirToDelete, new DeleteDirectorySettings{ Recursive = true, Force = true});
    });


Task("Restore")
    .Does(() =>
{
    NuGetRestore(solution);
});

Task("Build")
    .Does(() => 
    {    
        var msBuildSettings = new MSBuildSettings {
            Verbosity = verbosity
            , Configuration = configuration
        };

        MSBuild(solution, msBuildSettings
            .WithProperty("Description", "A simple launcher.")
        );
        
    });

Task("Inno-Setup")
    .Does(()=>
    {
        InnoSetup(inno_setup, new InnoSetupSettings { OutputDirectory = publishDir });
    });

Task("Unit-Test")      
    .Does(() =>
    {
        XUnit2(
        GetFiles("./src/Tests/Probel.Lanceur.UnitTest/bin/**/*UnitTest*.dll")
        );
    });

Task("Versioning")
    .Does(()=>
    {
        StartPowershellFile("./build/update-versions.ps1", args => args.Append("version", branchName));
    });

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    // .IsDependentOn("Versioning")
    .IsDependentOn("Build")
    .IsDependentOn("Unit-Test")
    .IsDependentOn("Inno-Setup");

RunTarget(target);