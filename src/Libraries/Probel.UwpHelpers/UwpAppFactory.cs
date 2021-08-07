using Probel.Lanceur.SharedKernel.Logs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Management.Deployment;

namespace Probel.UwpHelpers
{
    public class UwpAppFactory
    {
        #region Fields

        private readonly AppxPackageHelper _helper = new AppxPackageHelper();
        private readonly ILogService _log = LogServiceFactory.Get();

        #endregion Fields

        #region Enums

        private enum Hresult : uint
        {
            Ok = 0x0000,
        }

        [Flags]
        private enum Stgm : uint
        {
            Read = 0x0,
            DenyWrite = 0x20,
        }

        public enum PackageVersion
        {
            Windows10,
            Windows81,
            Windows8,
            Unknown
        }

        #endregion Enums

        #region Methods

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern Hresult SHCreateStreamOnFileEx(string fileName, Stgm grfMode, uint attributes, bool create, IStream reserved, out IStream stream);

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern Hresult SHLoadIndirectString(string pszSource, StringBuilder pszOutBuf, uint cchOutBuf, IntPtr ppvReserved);

        private PackageVersion InitPackageVersion(string[] namespaces)
        {
            var versionFromNamespace = new Dictionary<string, PackageVersion>
            {
                {"http://schemas.microsoft.com/appx/manifest/foundation/windows10", PackageVersion.Windows10},
                {"http://schemas.microsoft.com/appx/2013/manifest", PackageVersion.Windows81},
                {"http://schemas.microsoft.com/appx/2010/manifest", PackageVersion.Windows8},
            };

            foreach (var n in versionFromNamespace.Keys)
            {
                if (namespaces.Contains(n))
                {
                    return versionFromNamespace[n];
                }
            }

            _log.Warning($"Trying to get the package version of the UWP program, but a unknown UWP appmanifest version is returned.");

            return PackageVersion.Unknown;
        }

        /// http://www.hanselman.com/blog/GetNamespacesFromAnXMLDocumentWithXPathDocumentAndLINQToXML.aspx
        private string[] XmlNamespaces(string path)
        {
            XDocument z = XDocument.Load(path);
            if (z.Root != null)
            {
                var namespaces = z.Root.Attributes().
                    Where(a => a.IsNamespaceDeclaration).
                    GroupBy(
                        a => a.Name.Namespace == XNamespace.None ? string.Empty : a.Name.LocalName,
                        a => XNamespace.Get(a.Value)
                    ).Select(
                        g => g.First().ToString()
                    ).ToArray();
                return namespaces;
            }
            else
            {
                _log.Error($"|UWP|XmlNamespaces|{path}|Error occurred while trying to get the XML from {path}", new ArgumentNullException());
                return new string[] { };
            }
        }

        internal string LogoPathFromUri(UwpApp uwp, string theme, PackageVersion version)
        {
            // all https://msdn.microsoft.com/windows/uwp/controls-and-patterns/tiles-and-notifications-app-assets
            // windows 10 https://msdn.microsoft.com/en-us/library/windows/apps/dn934817.aspx
            // windows 8.1 https://msdn.microsoft.com/en-us/library/windows/apps/hh965372.aspx#target_size
            // windows 8 https://msdn.microsoft.com/en-us/library/windows/apps/br211475.aspx
            var uri = uwp.LogoUri;
            string path;
            if (uri.Contains("\\"))
            {
                path = Path.Combine(uwp.Location, uri);
            }
            else
            {
                // for C:\Windows\MiracastView etc
                path = Path.Combine(uwp.Location, "Assets", uri);
            }

            var extension = Path.GetExtension(path);
            if (extension != null)
            {
                var end = path.Length - extension.Length;
                var prefix = path.Substring(0, end);
                var paths = new List<string> { path };

                var scaleFactors = new Dictionary<PackageVersion, List<int>>
                    {
                        // scale factors on win10: https://docs.microsoft.com/en-us/windows/uwp/controls-and-patterns/tiles-and-notifications-app-assets#asset-size-tables,
                        { PackageVersion.Windows10, new List<int> { 100, 125, 150, 200, 400 } },
                        { PackageVersion.Windows81, new List<int> { 100, 120, 140, 160, 180 } },
                        { PackageVersion.Windows8, new List<int> { 100 } }
                    };

                if (scaleFactors.ContainsKey(version))
                {
                    foreach (var factor in scaleFactors[version])
                    {
                        paths.Add($"{prefix}.scale-{factor}{extension}");
                        paths.Add($"{prefix}.scale-{factor}_{theme}{extension}");
                        paths.Add($"{prefix}.{theme}_scale-{factor}{extension}");
                    }
                }

                paths = paths.OrderByDescending(x => x.Contains(theme)).ToList();
                var selected = paths.FirstOrDefault(File.Exists);
                if (!string.IsNullOrEmpty(selected))
                {
                    return selected;
                }
                else
                {
                    int appIconSize = 36;
                    var targetSizes = new List<int> { 16, 24, 30, 36, 44, 60, 72, 96, 128, 180, 256 }.AsParallel();
                    Dictionary<string, int> pathFactorPairs = new Dictionary<string, int>();

                    foreach (var factor in targetSizes)
                    {
                        string simplePath = $"{prefix}.targetsize-{factor}{extension}";
                        string suffixThemePath = $"{prefix}.targetsize-{factor}_{theme}{extension}";
                        string prefixThemePath = $"{prefix}.{theme}_targetsize-{factor}{extension}";

                        paths.Add(simplePath);
                        paths.Add(suffixThemePath);
                        paths.Add(prefixThemePath);

                        pathFactorPairs.Add(simplePath, factor);
                        pathFactorPairs.Add(suffixThemePath, factor);
                        pathFactorPairs.Add(prefixThemePath, factor);
                    }

                    paths = paths.OrderByDescending(x => x.Contains(theme)).ToList();
                    var selectedIconPath = paths.OrderBy(x =>
                    {
                        var val = pathFactorPairs.ContainsKey(x) ? pathFactorPairs[x] : default;
                        return Math.Abs(val - appIconSize);
                    }).FirstOrDefault(File.Exists);

                    if (!string.IsNullOrEmpty(selectedIconPath))
                    {
                        return selectedIconPath;
                    }
                    else
                    {
                        _log.Warning($"{uwp.UserModelId} can't find logo uri for {uri} in package location: {uwp.Location}");
                        return string.Empty;
                    }
                }
            }
            else
            {
                _log.Error($"|UWP|LogoPathFromUri|{uwp.Location}" +
                    $"|Unable to find extension from {uri} for {uwp.UserModelId} " +
                    $"in package location {uwp.Location}", new FileNotFoundException());
                return string.Empty;
            }
        }

        internal string LogoUriFromManifest(AppxPackageHelper.IAppxManifestApplication app, PackageVersion version)
        {
            var logoKeyFromVersion = new Dictionary<PackageVersion, string>
                {
                    { PackageVersion.Windows10, "Square44x44Logo" },
                    { PackageVersion.Windows81, "Square30x30Logo" },
                    { PackageVersion.Windows8, "SmallLogo" },
                };
            if (logoKeyFromVersion.ContainsKey(version))
            {
                var key = logoKeyFromVersion[version];
                app.GetStringValue(key, out string logoUri);
                return logoUri;
            }
            else
            {
                return string.Empty;
            }
        }

        internal string ResourceFromPri(string packageFullName, string resourceReference)
        {
            const string prefix = "ms-resource:";
            if (!string.IsNullOrWhiteSpace(resourceReference) && resourceReference.StartsWith(prefix))
            {
                // magic comes from @talynone
                // https://github.com/talynone/Wox.Plugin.WindowsUniversalAppLauncher/blob/master/StoreAppLauncher/Helpers/NativeApiHelper.cs#L139-L153
                string key = resourceReference.Substring(prefix.Length);
                string parsed;
                if (key.StartsWith("//"))
                {
                    parsed = prefix + key;
                }
                else if (key.StartsWith("/"))
                {
                    parsed = prefix + "//" + key;
                }
                else if (key.ToLower().Contains("resources"))
                {
                    parsed = prefix + key;
                }
                else
                {
                    parsed = prefix + "///resources/" + key;
                }

                var outBuffer = new StringBuilder(128);
                string source = $"@{{{packageFullName}? {parsed}}}";
                var capacity = (uint)outBuffer.Capacity;
                var hResult = SHLoadIndirectString(source, outBuffer, capacity, IntPtr.Zero);
                if (hResult == Hresult.Ok)
                {
                    var loaded = outBuffer.ToString();
                    if (!string.IsNullOrEmpty(loaded))
                    {
                        return loaded;
                    }
                    else
                    {
                        _log.Warning($"Can't load null or empty result pri {source} in uwp location {packageFullName}");
                        return string.Empty;
                    }
                }
                else
                {
                    // https://github.com/Wox-launcher/Wox/issues/964
                    // known hresult 2147942522:
                    // 'Microsoft Corporation' violates pattern constraint of '\bms-resource:.{1,256}'.
                    // for
                    // Microsoft.MicrosoftOfficeHub_17.7608.23501.0_x64__8wekyb3d8bbwe: ms-resource://Microsoft.MicrosoftOfficeHub/officehubintl/AppManifest_GetOffice_Description
                    // Microsoft.BingFoodAndDrink_3.0.4.336_x64__8wekyb3d8bbwe: ms-resource:AppDescription
                    var e = Marshal.GetExceptionForHR((int)hResult);
                    _log.Warning($"Load pri failed {source} with HResult {hResult} and location {packageFullName}", e);
                    return string.Empty;
                }
            }
            else
            {
                return resourceReference;
            }
        }

        public UwpApp Create(Package package)
        {
            var path = Path.Combine(package.InstalledLocation.Path, "AppxManifest.xml");

            var apps = new List<UwpApp>();
            const uint noAttribute = 0x80;
            const Stgm exclusiveRead = Stgm.Read | Stgm.DenyWrite;
            var hResult = SHCreateStreamOnFileEx(path, exclusiveRead, noAttribute, false, null, out IStream stream);

            if (hResult == Hresult.Ok)
            {
                List<AppxPackageHelper.IAppxManifestApplication> _apps = _helper.GetAppsFromManifest(stream);
                foreach (var app in _apps)
                {
                    app.GetAppUserModelId(out var tmpUserModelId);
                    app.GetAppUserModelId(out var tmpUniqueIdentifier);
                    app.GetStringValue("DisplayName", out var tmpDisplayName);
                    app.GetStringValue("Description", out var tmpDescription);
                    app.GetStringValue("BackgroundColor", out var tmpBackgroundColor);
                    app.GetStringValue("EntryPoint", out var tmpEntryPoint);
                    app.GetStringValue("Executable", out var tmpExecutable);

                    var executable = (string.IsNullOrWhiteSpace(tmpExecutable))
                        ? string.Empty
                        : Path.Combine(package.InstalledLocation.Path, tmpExecutable);

                    var a = new UwpApp
                    {
                        UserModelId = tmpUserModelId,
                        UniqueIdentifier = tmpUniqueIdentifier,
                        DisplayName = tmpDisplayName,
                        Description = tmpDescription,
                        BackgroundColor = tmpBackgroundColor,
                        EntryPoint = tmpEntryPoint,
                        Location = package.InstalledLocation.Path,
                        Executable = executable
                    };

                    var namespaces = XmlNamespaces(path);
                    var version = InitPackageVersion(namespaces);

                    a.LogoUri = LogoUriFromManifest(app, version);
                    a.DisplayName = ResourceFromPri(package.Id.FullName, a.DisplayName);
                    a.Description = ResourceFromPri(package.Id.FullName, a.Description);
                    a.LogoPath = LogoPathFromUri(a, "contrast-black", version); //contrast-black | contrast-white

                    apps.Add(a);
                }
            }
            return apps.Count == 0 ? new UwpApp() : apps[0];
        }

        public bool TrySetUwp(string userId, string alias, out Package package)
        {
            //Default value of the package.
            package = null;

            if (string.IsNullOrWhiteSpace(alias)) { return false; }
            else
            {
                try
                {
                    var srcDir = Path.GetDirectoryName(alias);

                    var rr = (from p in new PackageManager().FindPackagesForUser(userId)
                              where srcDir.StartsWith(p.InstalledPath)
                              select p).ToList();
                    var r = rr.FirstOrDefault();

                    if (r != null)
                    {
                        package = r;
                        return true;
                    }
                    else { return false; }
                }
                catch (FileNotFoundException) { return false; }
            }
        }

        #endregion Methods
    }
}