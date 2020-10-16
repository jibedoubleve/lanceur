using System;
using System.Collections.Generic;

namespace Probel.Lanceur.SharedKernel.Extensions
{
    public static class StringExtension
    {
        #region Methods

        public static bool IsNullOrEmpty(this string src) => string.IsNullOrEmpty(src);

        public static bool IsNullOrWhiteSpace(this string src) => string.IsNullOrWhiteSpace(src);

        public static string ToCsv(this IEnumerable<string> collection)
        {
            var result = string.Empty;
            var separator = ", ";
            foreach (var item in collection)
            {
                result += item + separator;
            }
            return result.Substring(0, result.Length - separator.Length);
        }

        public static Version ToVersion(this string src)
        {
            if (Version.TryParse(src, out var version)) { return version; }
            else { return new Version(); }
        }

        #endregion Methods
    }
}