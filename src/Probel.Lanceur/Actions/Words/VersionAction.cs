using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace Probel.Lanceur.Actions.Words
{
    [UiAction]
    public class VersionAction : BaseUiAction
    {
        #region Methods

        protected override void DoExecute(string arg)
        {
            var asm = Assembly.GetExecutingAssembly();
            var version = asm.GetName().Version.ToString();
            var fileVersion = FileVersionInfo.GetVersionInfo(asm.Location).FileVersion.ToString();

            var semver = FileVersionInfo.GetVersionInfo(asm.Location).ProductVersion.ToString();
            var semverSplit = semver.Split(new string[] { "+" }, StringSplitOptions.RemoveEmptyEntries);
            semver = semverSplit.Length > 0 ? semverSplit[0] : semver;

            var nl = Environment.NewLine;
            var msg = $"Version: {version}{nl}File Version: {fileVersion}{nl}SemVer: {semver}{nl}Author: JB Wautier";
            MessageBox.Show(msg, "Probel Lanceur", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion Methods
    }
}