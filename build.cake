
///////////////////////////////////////////////////////////////////////////////
// TOOLS / ADDINS
///////////////////////////////////////////////////////////////////////////////
#tool vswhere
#tool GitVersion.CommandLine
#tool xunit.runner.console
#tool gitreleasemanager

#addin "Cake.Powershell"
#addin Cake.Figlet
#addin Cake.Powershell
#addin "Cake.FileHelpers"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

using IoPath = System.IO.Path;

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////
var repoName = "Lanceur";

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var verbosity = Argument("verbosity", Verbosity.Minimal);

/* This list contains the path of the assets to release.
 * It is cleared and filled into task "Zip" and used into
 * the task "Release-GitHub".
 */
var assets = new List<string>(); 

/* These arguments are used to build the nuget packages
 */
var latestInstallationPath = VSWhereLatest(new VSWhereLatestSettings { IncludePrerelease = true });
var msBuildPath = latestInstallationPath.Combine("./MSBuild/Current/Bin");
var msBuildPathExe = msBuildPath.CombineWithFilePath("./MSBuild.exe");

/* These arguments are used for Evernote Plugin. It contains the
 * information to build the json file with API key and host
 */
 var evernote_cfg_dest = @"%userprofile%\Documents\Secrets\evenrote.plugin.config.json";

///////////////////////////////////////////////////////////////////////////////
// PREPARATION
///////////////////////////////////////////////////////////////////////////////

//Files
var solution     = "./src/Probel.Lanceur.sln";
var publishDir   = "./Publish/";
var inno_setup   = "./setup.iss";

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
            Verbosity = verbosity,
            Configuration = configuration,
            PlatformTarget = PlatformTarget.x64
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

        var dir = new DirectoryInfo(binDirectory + @"/../../../plugins/");        
        foreach(var d in dir.GetDirectories())
        {
            var pluginBin = d.FullName + @"\bin\Release\";
            var pluginDest = publishDir + "/" + d.Name.Replace("Probel.Lanceur.Plugin.","plugin-") + "-" + gitVersion.SemVer + ".bin.zip";
            assets.Add(pluginDest);

            Information("ZIPPING PLUGIN");
            Information("    bin: {0}", pluginBin);
            Information("   dest: {0}", pluginDest);

            Zip(pluginBin, pluginDest);
        }

        var repo = new DirectoryInfo(binDirectory + @"/../../../repositories/");
        foreach(var d in repo.GetDirectories())
        {
            var repoBin = d.FullName + @"\bin\Release\";
            var repoDest = publishDir + "/" + d.Name.Replace("Probel.Lanceur.Repository.","repository-") + "-" + gitVersion.SemVer + ".bin.zip";
            assets.Add(repoDest);

            Information("ZIPPING REPOSITORY");
            Information("    bin: {0}", repoBin);
            Information("   dest: {0}", repoDest);

            Zip(repoBin, repoDest);
        }
    
});  

Task("Evernote-file")
    .Does(()=>{
        var host = EnvironmentVariable("EVERNOTE_API_HOST");
        var key = EnvironmentVariable("EVERNOTE_API_KEY");
        var json = $"{{\"host\":\"{host}\",\"key\":\"{key}\"}}";     

        
        var dir = new DirectoryInfo(binDirectory + @"/../../../plugins/Probel.Lanceur.Plugin.Evernote\");
        var pluginDest = dir.FullName + @"\bin\Release\api.json";

        Information("Configuration file built at: {0}", pluginDest);

        FileWriteText(pluginDest, json);
});

Task("Inno-Setup")
    .Does(() => {
        
        var binPluginDir = $"./src/plugins/Probel.Lanceur.Plugin.{{0}}/bin/{configuration}/";
        var binRepoDir   = $"./src/repositories/Probel.Lanceur.Repository.{{0}}/bin/{configuration}/";

        var path      = MakeAbsolute(Directory(binDirectory)).FullPath + "\\";
        var pluginDir = MakeAbsolute(Directory(binPluginDir)).FullPath + "\\";
        var repoDir   = MakeAbsolute(Directory(binRepoDir)).FullPath + "\\";


        var plugins   = new string[] { "spotify", "calculator", "clipboard", "evernote" };    
        var repos     = new string[]{ "Win32Search", "UwpSearch" };
        
        Information("Bin path   : {0}: ", path);
        // Information("Plugin path: {0}: ", pluginDir);
        var p = 0;
        var r = 0;

        InnoSetup(inno_setup, new InnoSetupSettings { 
            OutputDirectory = publishDir,
            Defines = new Dictionary<string, string> {
                { "MyAppVersion", gitVersion.SemVer },
                { "BinDirectory", path },
                // PLUGINS
                { "SpotifyPluginDir"   , string.Format(pluginDir, plugins[p++]) },
                { "CalculatorPluginDir", string.Format(pluginDir, plugins[p++]) },
                { "ClipboardPluginDir" , string.Format(pluginDir, plugins[p++]) },
                { "EvernotePluginDir"  , string.Format(pluginDir, plugins[p++]) },
                // REPOSITORIES
                { "Win32SearchAPI", string.Format(repoDir, repos[r++])},
                { "UwpSearchAPI", string.Format(repoDir, repos[r++])},
            }
        });
});

Task("Chocolatey")
    .Does(()=>{
        //https://github.com/SharpeRAD/Cake.Powershell#usage
        StartPowershellScript("./build-package.ps1", new PowershellSettings()
            .UseWorkingDirectory("./chocolatey")
            .WithArguments(args => { 
                args.Append("configuration", configuration);
                args.Append("version", gitVersion.MajorMinorPatch);
        })
      );  
});

Task("Release-GitHub")
    .Does(()=>{
        //https://stackoverflow.com/questions/42761777/hide-services-passwords-in-cake-build
        var token     = EnvironmentVariable("CAKE_PUBLIC_GITHUB_TOKEN");
        var owner     = EnvironmentVariable("CAKE_PUBLIC_GITHUB_USERNAME");
        var milestone = gitVersion.MajorMinorPatch;
        var name      = gitVersion.SemVer;
        var pre       = gitVersion.SemVer.Contains("alpha") || gitVersion.SemVer.Contains("beta");

        var pDir = IoPath.GetFullPath(publishDir);


        var assets    = IoPath.Combine(pDir, "lanceur." + gitVersion.SemVer + ".bin.zip"          ) + "," 
                      + IoPath.Combine(pDir, "lanceur." + gitVersion.SemVer + ".setup.exe"        ) + "," 
                      + IoPath.Combine(pDir, "plugin-calculator-" + gitVersion.SemVer + ".bin.zip") + "," 
                      + IoPath.Combine(pDir, "plugin-spotify-" + gitVersion.SemVer + ".bin.zip"   ) + "," 
                      + IoPath.Combine(pDir, "plugin-clipboard-" + gitVersion.SemVer + ".bin.zip" ) + "," 
                      + IoPath.Combine(pDir, "plugin-evernote-" + gitVersion.SemVer + ".bin.zip"  );

        Information("Assets: {0}", assets);

        StartPowershellFile(@".\Scripts\GitReleaseManager.ps1", args => {
            args.Append("token", token)
                .Append("owner", owner)
                .Append("milestone", milestone)
                .Append("name", name)
                .Append("assets", assets)
                .Append("repo", "lanceur")
                .Append("isPrerelease", pre.ToString());
        });
});

Task("pack-plugin")
    .ContinueOnError()
    .Does(() =>
{
    EnsureDirectoryExists(Directory(publishDir));

    var msBuildSettings = new MSBuildSettings {
        Verbosity = verbosity
        , ToolPath = msBuildPathExe
        , Configuration = configuration
    };

    var project = "./src/Infrastructure/Probel.Lanceur.Plugin/Probel.Lanceur.Plugin.csproj";
    MSBuild(project, msBuildSettings
      .WithTarget("pack")
      .WithProperty("NoBuild", "true")
      .WithProperty("IncludeBuildOutput", "true")
      .WithProperty("PackageOutputPath", "../../" + publishDir)
      .WithProperty("RepositoryBranch", branchName)
      .WithProperty("RepositoryCommit", gitVersion.Sha)
      .WithProperty("Description", "This is the library to use to create plugins for Lanceur")
      .WithProperty("Version", gitVersion.MajorMinorPatch)
      .WithProperty("AssemblyVersion", gitVersion.AssemblySemVer)
      .WithProperty("FileVersion", gitVersion.AssemblySemFileVer)
      .WithProperty("InformationalVersion", gitVersion.InformationalVersion)
    );
});

Task("pack-repository")
    .ContinueOnError()
    .Does(() =>
{
    EnsureDirectoryExists(Directory(publishDir));

    var msBuildSettings = new MSBuildSettings {
        Verbosity = verbosity
        , ToolPath = msBuildPathExe
        , Configuration = configuration
    };

    var project = "./src/Infrastructure/Probel.Lanceur.Repositories/Probel.Lanceur.Repositories.csproj";
    MSBuild(project, msBuildSettings
      .WithTarget("pack")
      .WithProperty("NoBuild", "true")
      .WithProperty("IncludeBuildOutput", "true")
      .WithProperty("PackageOutputPath", "../../" + publishDir)
      .WithProperty("RepositoryBranch", branchName)
      .WithProperty("RepositoryCommit", gitVersion.Sha)
      .WithProperty("Description", "This is the library to use to create plugins for Lanceur")
      .WithProperty("Version", gitVersion.MajorMinorPatch)
      .WithProperty("AssemblyVersion", gitVersion.AssemblySemVer)
      .WithProperty("FileVersion", gitVersion.AssemblySemFileVer)
      .WithProperty("InformationalVersion", gitVersion.InformationalVersion)
    );
});

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("pack-plugin")
    .IsDependentOn("pack-repository")
    .IsDependentOn("Unit-Test")
    .IsDependentOn("Evernote-file")
    .IsDependentOn("Zip")
    .IsDependentOn("Inno-Setup")
    .IsDependentOn("Chocolatey");

Task("Choco")    
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("pack-plugin")
    .IsDependentOn("pack-repository")
    .IsDependentOn("Unit-Test")
    .IsDependentOn("Evernote-file")
    .IsDependentOn("Zip")
    .IsDependentOn("Chocolatey");

Task("Github")    
    .IsDependentOn("Default")
    .IsDependentOn("Release-GitHub");

/* It does nothing. Just to display versions
 */
Task("ver");

RunTarget(target);