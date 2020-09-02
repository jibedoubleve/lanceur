using System.Collections.Generic;
using System.Linq;

namespace Probel.Lanceur.Infrastructure.ServicesImpl.MacroManagement
{
    internal class Macros
    {
        #region Fields

        private static IEnumerable<string> MacrosList = null;

        #endregion Fields

        #region Properties

        /// <summary>
        /// A "MultiWord" that executes multiple MagicWords.Set the Filename field to @MULTI@ and set the parameters field to a @ delimited list of MagicWords to be executed.
        /// </summary>
        public static string Multi => "@MULTI@";

        #endregion Properties

        #region Methods

        private static void FillMacroList()
        {
            var result = new List<string>();

            var properties = (from p in typeof(Macros).GetProperties()
                              where p.CanRead
                              select p);
            foreach (var property in properties)
            {
                result.Add((string)property.GetValue(null));
            }
            MacrosList = result;
        }

        public static bool Has(string macro)
        {
            macro = macro ?? string.Empty;

            if (MacrosList == null) { FillMacroList(); }

            var result = (from m in MacrosList
                          where m.ToUpper() == macro.ToUpper()
                          select m).Count() > 0;
            return result;
        }

        #endregion Methods
    }
}