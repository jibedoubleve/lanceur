
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
var inno_setup = "./setup.iss";

GitVersion gitVersion = GitVersion(new GitVersionSettings 
{ 
    OutputType = GitVersionOutput.Json,
    UpdateAssemblyInfo  = true,
    UpdateAssemblyInfoFilePath  = "./src/Version.cs",
});
var branchName = gitVersion.BranchName;
var binDirectory = $"./src/Probel.Lanceur/bin/{configuration}/";

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

    Information("Configuration             : {0}", configuration);
    Information("Branch                    : {0}", branchName);
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
    .Does(()=> {
        var dirToDelete = GetDirectories("./**/obj")
                            .Concat(GetDirectories("./**/bin"))
                            .Concat(GetDirectories("./**/Publish"));
        DeleteDirectories(dirToDelete, new DeleteDirectorySettings{ Recursive = true, Force = true});
});


Task("Restore")
    .Does(() => {
        NuGetRestore(solution);
});

Task("Build")
    .Does(() => {    
        var msBuildSettings = new MSBuildSettings {
            Verbosity = verbosity
            , Configuration = configuration
        };

        MSBuild(solution, msBuildSettings
            .WithProperty("Description", "A simple launcher.")
        );
        
});

Task("Unit-Test")      
    .Does(() => {
        XUnit2(
            GetFiles("./src/Tests/Probel.Lanceur.UnitTest/bin/**/*UnitTest*.dll")
        );
});

Task("Zip")
    .Does(()=> {
        var zipName = publishDir + "/lanceur." + gitVersion.SemVer + ".bin.zip";
        Information("Path   : {0}: ", zipName);
        Information("Bin dir: {0}", binDirectory);

        EnsureDirectoryExists(Directory(publishDir));
        Zip(binDirectory, zipName);
    
});  

Task("Inno-Setup")
    .Does(() => {
        var path = binDirectory.Replace("/", "\\").TrimStart('.').TrimStart('\\');
        Information("Path: {0}: ", path);

        InnoSetup(inno_setup, new InnoSetupSettings { 
            OutputDirectory = publishDir,
            Defines = new Dictionary<string, string> {
                 { "MyAppVersion", gitVersion.SemVer },
                 { "BinDirectory", path }
            }
        });
});

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("Unit-Test")
    .IsDependentOn("Zip")
    .IsDependentOn("Inno-Setup");

RunTarget(target);