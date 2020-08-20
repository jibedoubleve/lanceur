using System;

namespace Probel.Lanceur.Infrastructure.Helpers
{
    public static class StringHelpers
    {
        #region Methods

        public static Version ToVersion(this string src)
        {
            if (Version.TryParse(src, out var version)) { return version; }
            else { return new Version(); }
        }

        #endregion Methods
    }
}