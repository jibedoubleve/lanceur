using System;
using System.IO;

namespace Probel.UwpHelpers
{
    public class UwpApp
    {
        #region Properties

        public string AppxManifestPath => (string.IsNullOrWhiteSpace(Location) == false) ? Path.Combine(Location, "AppxManifest.xml") : string.Empty;
        public string BackgroundColor { get; internal set; }
        public string Description { get; internal set; }
        public string DisplayName { get; set; }
        public string EntryPoint { get; internal set; }
        public string Executable { get; internal set; }
        public Uri IconUri { get; internal set; }
        public bool IsEmpty => string.IsNullOrEmpty(UserModelId);
        public string Location { get; internal set; }
        public string LogoPath { get; set; }
        public string LogoUri { get; internal set; }
        public string UniqueIdentifier { get; internal set; }
        public string UserModelId { get; internal set; }

        #endregion Properties
    }
}