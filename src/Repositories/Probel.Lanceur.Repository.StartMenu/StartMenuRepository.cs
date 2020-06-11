using Probel.Lanceur.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Probel.Lanceur.Repository.StartMenu
{
    public class StartMenuRepository : AliasRepositoryBase
    {
        #region Fields

        private readonly Find _find;

        #endregion Fields

        #region Constructors

        public StartMenuRepository()
        {
            _find = new Find();
        }

        #endregion Constructors

        #region Methods

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
            var result = (from r in GetAliases()
                          where r.Name.ToLower().Contains(criterion.ToLower())
                          orderby r.Name
                          select r);
            return result;
        }

        protected override void Initialise()
        {
            _find.Log = Log;
        }

        #endregion Methods
    }
}