using Probel.Lanceur.Core.Entities;
using System;
using System.Collections.Generic;

namespace Probel.Lanceur.Core.Helpers
{
    public static class CmdLineHelper
    {
        #region Methods

        //https://stackoverflow.com/questions/2471588/how-to-get-index-using-linq
        ///<summary>Finds the index of the first item matching an expression in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="predicate">The expression to test the items against.</param>
        ///<returns>The index of the first matching item, or -1 if no items match.</returns>
        private static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null) { throw new ArgumentNullException("items"); }
            if (predicate == null) { throw new ArgumentNullException("predicate"); }

            var retVal = 0;
            foreach (var item in items)
            {
                if (predicate(item)) { return retVal; }

                retVal++;
            }
            return -1;
        }

        public static int Index(this Cmdline src, string paramName)
        {
            var name = paramName.ToLower();
            return src.SplitedParameters.FindIndex(n => n.ToLower() == name);
        }

        #endregion Methods
    }
}