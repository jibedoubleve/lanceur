using Probel.Lanceur.Models;
using System.Collections.Generic;
using System.Linq;

namespace Probel.Lanceur.Services
{
    internal static class SessionQueries
    {
        #region Methods

        public static ShortcutSessionModel GetSession(this IEnumerable<ShortcutSessionModel> src, long id)
        {
            var r = (from s in src
                     where s.Id == id
                     select s).SingleOrDefault();
            return r;
        }        

        #endregion Methods
    }
}