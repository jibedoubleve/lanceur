
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
var latestInstallationPath = VSWhereLatest(new VSWhereLatestSettings { IncludePrerelease = true });
var msBuildPath            = latestInstallationPath.Combine("./MSBuild/Current/Bin");
var msBuildPathExe         = msBuildPath.CombineWithFilePath("./MSBuild.exe");

//Files
var solution   = "./src/Probel.Lanceur.sln";
var publishDir = "/Release/project-lanceur/inno";
var inno_setup = "./build/setup.iss";

GitVersion gitVersion = GitVersion(new GitVersionSettings { OutputType = GitVersionOutput.Json });
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

    Information("Branch                 : {0}", branchName);
    Information("Configuration          : {0}", configuration);
    Information("MSBuildPath            : {0}", msBuildPath);
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
Task("Build")
    .Does(() => 
    {    
        var msBuildSettings = new MSBuildSettings {
            Verbosity = verbosity
            , ToolPath = msBuildPathExe
            , Configuration = configuration
        };

        MSBuild(solution, msBuildSettings
            // .SetMaxCpuCount(0)
            .WithProperty("Description", "A simple launcher.")
        );
        
    });
Task("Inno-Setup")
    .Does(()=>
    {
        InnoSetup(inno_setup);
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
    .IsDependentOn("Versioning")
    .IsDependentOn("Build")
    .IsDependentOn("Unit-Test")
    .IsDependentOn("Inno-Setup");

RunTarget(target);