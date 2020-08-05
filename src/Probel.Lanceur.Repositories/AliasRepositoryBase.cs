using Probel.Lanceur.Infrastructure;
using System.Collections.Generic;

namespace Probel.Lanceur.Repositories
{
    public abstract class AliasRepositoryBase : IAliasRepository
    {
        #region Properties

        protected ILogService Log { get; private set; }
        protected IRepositorySettings Settings { get; private set; }
        public string Keyword { get; protected set; }

        #endregion Properties

        #region Methods

        protected virtual void Initialise()
        {
            /* By default it does nothing.
             */
        }

        public abstract IEnumerable<RepositoryAlias> GetAliases();

        public abstract IEnumerable<RepositoryAlias> GetAliases(string criterion);

        public void Initialise(IRepositoryContext context)
        {
            Log = context.LogService;
            Settings = context.Settings;
            Initialise();
        }

        public string NormaliseQuery(string query)
        {
            if (query.StartsWith(Keyword))
            {
                var temp = query.ToLower();
                var result = temp.Substring(Keyword.Length);
                return result;
            }
            else { return query; }
        }

        #endregion Methods
    }
}