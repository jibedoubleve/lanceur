using DuoVia.FuzzyStrings;
using Probel.Lanceur.Repositories;
using Probel.Lanceur.SharedKernel.Helpers;
using Probel.UwpHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Probel.Lanceur.Repository.UwpSearch
{
    public class UwpSearchAPIRepository : AliasRepositoryBase
    {
        #region Fields

        private readonly AppxLister _find = new AppxLister();

        #endregion Fields

        #region Methods

        protected override void Initialise()
        {

            Task.Run(() =>
            {
                using (new Benchmark("Load UWP Aliases")) { _find.All(); }
            });
        }

        public override IEnumerable<RepositoryAlias> GetAliases()
        {
            var results = _find.All();

            var found = (from r in results
                         where string.IsNullOrWhiteSpace(r.UniqueIdentifier) == false //r.Name.ToLower().Contains("uninstall") == false
                         orderby r.DisplayName
                         select new RepositoryAlias
                         {
                             ExecutionCount = 0,
                             FileName = r.Executable,
                             IsPackaged = true,
                             UniqueIdentifier = r.UniqueIdentifier,
                             Kind = "Flash",
                             Name = r.DisplayName,
                             Icon = r.LogoPath,
                         });
            return found.ToList();
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