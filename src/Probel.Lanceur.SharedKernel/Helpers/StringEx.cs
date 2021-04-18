using System.Collections.Generic;
using System.Text;

namespace Probel.Lanceur.SharedKernel.Helpers
{
    public static class StringEx
    {
        #region Methods

        public static string AsComaString(this IEnumerable<string> collection)
        {
            var sb = new StringBuilder();
            foreach (var item in collection)
            {
                sb.Append(item);
                sb.Append(", ");
            }
            return sb.ToString().TrimEnd(' ', ',');
        }

        #endregion Methods
    }
}