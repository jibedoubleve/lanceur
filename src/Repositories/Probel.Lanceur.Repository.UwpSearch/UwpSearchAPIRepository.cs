using Probel.Lanceur.Infrastructure.Helpers;
using Probel.Lanceur.Repositories;
using Probel.Lanceur.Repository.UwpSearch.Core;
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
            _find.Log = Log;
            _find.Log = Log;

            Task.Run(() =>
            {
                using (new Benchmark("Load UWP Aliases")) { _find.All(); }
            });
        }

        public override IEnumerable<RepositoryAlias> GetAliases()
        {
            var results = _find.All();

            var found = (from r in results.AsParallel()
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
            return found;
        }


        public override IEnumerable<RepositoryAlias> GetAliases(string criterion)
        {
            var result = (from r in GetAliases()
                          where r.Name.ToLower().Contains(criterion.ToLower())
                          orderby r.Name
                          select r);
            return result;
        }

        #endregion Methods
    }
}