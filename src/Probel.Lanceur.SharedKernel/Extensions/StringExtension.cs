using System;

namespace Probel.Lanceur.SharedKernel.Extensions
{
    public static class StringExtension
    {
        #region Methods

        public static bool IsNullOrEmpty(this string src) => string.IsNullOrEmpty(src);

        public static bool IsNullOrWhiteSpace(this string src) => string.IsNullOrWhiteSpace(src);
        public static Version ToVersion(this string src)
        {
            if (Version.TryParse(src, out var version)) { return version; }
            else { return new Version(); }
        }

        #endregion Methods
    }
}