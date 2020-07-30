using DuoVia.FuzzyStrings;
using Probel.Lanceur.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Probel.Lanceur.Repository.Win32Search
{
    public class WinSearchAPIRepository : AliasRepositoryBase
    {
        #region Fields

        private readonly Find _find;

        #endregion Fields

        #region Constructors

        public WinSearchAPIRepository()
        {
            _find = new Find();
        }

        #endregion Constructors

        #region Methods

        protected override void Initialise()
        {
            _find.Log = Log;
        }

        public override IEnumerable<RepositoryAlias> GetAliases()
        {
            var result = new List<AppInfo>();

            result.AddRange(_find.InStartMenuPrograms());
            result.AddRange(_find.InRegistry());

            var found = (from r in result.Cast()
                         where r.Name.ToLower().Contains("uninstall") == false
                         orderby r.Name
                         select r);
            return found;
        }

        public override IEnumerable<RepositoryAlias> GetAliases(string criterion)
        {
            criterion = criterion?.ToLower() ?? string.Empty;
            var aliases = GetAliases();

            if (string.IsNullOrEmpty(criterion)) { return aliases; }
            else
            {
                //https://stackoverflow.com/questions/5859561/getting-the-closest-string-match
                Parallel.ForEach(aliases, r => r.SearchScore = r.Name.ToLower().FuzzyMatch(criterion));
                var result = (from r in aliases
                              where r.SearchScore >= 0.15
                              orderby r.SearchScore descending, r.Name
                              select r).ToList();
                return result;
            }
        }

        #endregion Methods
    }
}