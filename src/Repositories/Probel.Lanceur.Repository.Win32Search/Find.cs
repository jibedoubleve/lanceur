using Microsoft.Win32;
using Probel.Lanceur.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;

namespace Probel.Lanceur.Repository.Win32Search
{
    public class Find
    {
        #region Fields

        private const string ApplicationReferenceExtension = "appref-ms";
        private const string ExeExtension = "exe";

        #endregion Fields

        #region Properties

        public ILogService Log
        {
            get; set;
        }

        private string[] ProgramSuffixes { get; set; } = { "bat", "appref-ms", "exe", "lnk" };

        #endregion Properties

        #region Methods

        public IEnumerable<AppInfo> InRegistry() => InRegistry(ProgramSuffixes);

        public IEnumerable<AppInfo> InStartMenuPrograms() => InStartMenuPrograms(ProgramSuffixes);

        private string GetProgramPathFromRegistrySubKeys(RegistryKey root, string subkey)
        {
            var path = string.Empty;
            try
            {
                using (var key = root.OpenSubKey(subkey))
                {
                    if (key == null)
                        return string.Empty;

                    var defaultValue = string.Empty;
                    path = key.GetValue(defaultValue) as string;
                }

                if (string.IsNullOrEmpty(path))
                    return string.Empty;

                // fix path like this: ""\"C:\\folder\\executable.exe\""
                return path = path.Trim('"', ' ');
            }
            catch (Exception e) when (e is SecurityException || e is UnauthorizedAccessException)
            {
                LogWarn($"|Win32|GetProgramPathFromRegistrySubKeys|{path}" + $"|Permission denied when trying to load the program from {path}");

                return string.Empty;
            }
        }

        private IEnumerable<AppInfo> GetProgramsFromRegistry(Microsoft.Win32.RegistryKey root)
        {
            return root
                    .GetSubKeyNames()
                    .Select(x => GetProgramPathFromRegistrySubKeys(root, x))
                    .Distinct()
                    .Select(x => AppInfo.FromPath(x));
        }

        private IEnumerable<AppInfo> InRegistry(string[] suffixes)
        {
            // https://msdn.microsoft.com/en-us/library/windows/desktop/ee872121
            const string appPaths = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths";
            var programs = new List<AppInfo>();
            using (var root = Registry.LocalMachine.OpenSubKey(appPaths))
            {
                if (root != null)
                {
                    programs.AddRange(GetProgramsFromRegistry(root));
                }
            }
            using (var root = Registry.CurrentUser.OpenSubKey(appPaths))
            {
                if (root != null)
                {
                    programs.AddRange(GetProgramsFromRegistry(root));
                }
            }
            return programs;
        }

        private IEnumerable<AppInfo> InStartMenuPrograms(string[] suffixes)
        {
            //var disabledProgramsList = Main._settings.DisabledProgramSources;

            var directory1 = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
            var directory2 = Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms);
            var paths1 = ProgramPaths(directory1, suffixes);
            var paths2 = ProgramPaths(directory2, suffixes);

            var r = (from p in paths1.Concat(paths2)
                     select AppInfo.FromPath(p)).ToList();
            return r;
        }

        private void LogWarn(string message)
        {
            if (Log != null) { Log.Warning(message); }
            else { Trace.WriteLine(message); }
        }

        private IEnumerable<string> ProgramPaths(string directory, string[] suffixes)
        {
            if (!Directory.Exists(directory))
            {
                return Array.Empty<string>();
            }

            var files = new List<string>();
            var folderQueue = new Queue<string>();
            folderQueue.Enqueue(directory);

            do
            {
                var currentDirectory = folderQueue.Dequeue();
                try
                {
                    foreach (var suffix in suffixes)
                    {
                        try
                        {
                            files.AddRange(Directory.EnumerateFiles(currentDirectory, $"*.{suffix}", SearchOption.TopDirectoryOnly));
                        }
                        catch (DirectoryNotFoundException)
                        {
                            LogWarn($"|Win32|ProgramPaths|{currentDirectory}" + "|The directory trying to load the program from does not exist");
                        }
                    }
                }
                catch (Exception e) when (e is SecurityException || e is UnauthorizedAccessException)
                {
                    LogWarn($"|Win32|ProgramPaths|{currentDirectory}" + $"|Permission denied when trying to load programs from {currentDirectory}");
                }

                try
                {
                    foreach (var childDirectory in Directory.EnumerateDirectories(currentDirectory, "*", SearchOption.TopDirectoryOnly))
                    {
                        folderQueue.Enqueue(childDirectory);
                    }
                }
                catch (Exception e) when (e is SecurityException || e is UnauthorizedAccessException)
                {
                    LogWarn($"|Win32|ProgramPaths|{currentDirectory}" + $"|Permission denied when trying to load programs from {currentDirectory}");
                }
            } while (folderQueue.Any());

            return files;
        }

        #endregion Methods
    }
}