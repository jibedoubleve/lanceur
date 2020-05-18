
///////////////////////////////////////////////////////////////////////////////
// TOOLS / ADDINS
///////////////////////////////////////////////////////////////////////////////
#tool vswhere
#tool GitVersion.CommandLine
#tool xunit.runner.console
#tool gitreleasemanager

#addin Cake.Figlet
#addin Cake.Powershell
#addin "Cake.FileHelpers"

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
var solution   = "./src/Probel.Lanceur.sln";
var publishDir = "./Publish/";
var inno_setup = "./setup.iss";
var binPluginDir    = $"./src/plugins/Probel.Lanceur.Plugin.{{0}}/bin/{configuration}/";

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
            var dest = publishDir + "/" + d.Name.Replace("Probel.Lanceur.Plugin.","plugin-") + "-" + gitVersion.SemVer + ".bin.zip";
            assets.Add(dest);

            Information("Zipping plugin: {0}", dest);
            Information("  pluginBin   : {0}", pluginBin);
            Information("  dest        : {0}", dest);

            Zip(pluginBin, dest);
        }
    
});  

Task("Evernote-file")
    .Does(()=>{
        var host = EnvironmentVariable("EVERNOTE_API_HOST");
        var key = EnvironmentVariable("EVERNOTE_API_KEY");
        var json = $"{{\"host\":\"{host}\",\"key\":\"{key}\"}}";     

        
        var dir = new DirectoryInfo(binDirectory + @"/../../../plugins/Probel.Lanceur.Plugin.Evernote\");
        var dest = dir.FullName + @"\bin\Release\api.json";

        Information("Configuration file built at: {0}", dest);

        FileWriteText(dest, json);
});

Task("Inno-Setup")
    .Does(() => {
        var path          = MakeAbsolute(Directory(binDirectory)).FullPath + "\\";
        var pluginDir     = MakeAbsolute(Directory(binPluginDir)).FullPath + "\\";
        var plugins       = new string[] { "spotify", "calculator", "clipboard", "evernote" };         
        
        Information("Bin path   : {0}: ", path);
        Information("Plugin path: {0}: ", pluginDir);

        InnoSetup(inno_setup, new InnoSetupSettings { 
            OutputDirectory = publishDir,
            Defines = new Dictionary<string, string> {
                { "MyAppVersion", gitVersion.SemVer },
                { "BinDirectory", path },
                { "SpotifyPluginDir", String.Format(pluginDir, plugins[0]) },
                { "CalculatorPluginDir", String.Format(pluginDir, plugins[1]) },
                { "ClipboardPluginDir", String.Format(pluginDir, plugins[2]) },
                { "EvernotePluginDir", String.Format(pluginDir, plugins[3]) }
            }
        });
});

Task("Release-GitHub")
    .Does(()=>{
        //https://stackoverflow.com/questions/42761777/hide-services-passwords-in-cake-build
        var token = EnvironmentVariable("CAKE_PUBLIC_GITHUB_TOKEN");
        var owner = EnvironmentVariable("CAKE_PUBLIC_GITHUB_USERNAME");

        var stg = new GitReleaseManagerCreateSettings 
        {
            Milestone         = "V" + gitVersion.MajorMinorPatch,            
            Name              = gitVersion.SemVer,
            Prerelease        = gitVersion.SemVer.Contains("alpha"),
            Assets            = publishDir + "/lanceur." + gitVersion.SemVer + ".bin.zip," 
                              + publishDir + "/lanceur." + gitVersion.SemVer + ".setup.exe,"
                              + publishDir + "/plugin-calculator-" + gitVersion.SemVer + ".bin.zip," 
                              + publishDir + "/plugin-spotify-" + gitVersion.SemVer + ".bin.zip," 
                              + publishDir + "/plugin-clipboard-" + gitVersion.SemVer + ".bin.zip," 
                              + publishDir + "/plugin-evernote-" + gitVersion.SemVer + ".bin.zip" 
        };

        GitReleaseManagerCreate(token, owner, "Lanceur", stg);  
});

Task("Pack")
    .ContinueOnError()
    .Does(() =>
{
    EnsureDirectoryExists(Directory(publishDir));

    var msBuildSettings = new MSBuildSettings {
        Verbosity = verbosity
        , ToolPath = msBuildPathExe
        , Configuration = configuration
    };

    var project = "./src/Probel.Lanceur.Plugin/Probel.Lanceur.Plugin.csproj";
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
    .IsDependentOn("Pack")
    .IsDependentOn("Unit-Test")
    .IsDependentOn("Evernote-file")
    .IsDependentOn("Zip")
    .IsDependentOn("Inno-Setup");

Task("Github")    
    .IsDependentOn("Default")
    .IsDependentOn("Release-GitHub");

RunTarget(target);