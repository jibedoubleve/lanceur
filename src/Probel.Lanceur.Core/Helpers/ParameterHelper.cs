using System.Text.RegularExpressions;

namespace Probel.Lanceur.Core.Helpers
{
    public static class ParameterHelper
    {
        #region Methods

        public static string ToNormalisedParameter(this string src) => Regex.Replace(src, @"\$[a-zA-Z]\$", m => m.ToString().ToUpper());

        #endregion Methods
    }
}