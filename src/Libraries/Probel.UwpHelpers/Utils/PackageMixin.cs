using Probel.Lanceur.SharedKernel.Logs;
using Windows.ApplicationModel;

namespace Probel.UwpHelpers.Utils
{
    public static class PackageMixin
    {
        #region Methods

        public static string GetInstallationPath(this Package package, ILogService log = null)
        {
            var path = package.InstalledLocation.Path;
            return path;
        }

        public static bool HasInstallationPath(this Package package, ILogService log = null)
        {
            try
            {
                var res = package.InstalledLocation.Path;
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion Methods
    }
}