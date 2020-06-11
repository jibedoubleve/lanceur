using Probel.Lanceur.Infrastructure;
using System.Collections.Generic;

namespace Probel.Lanceur.Repositories
{
    public abstract class AliasRepositoryBase : IAliasRepository
    {
        #region Properties

        public string Keyword { get; protected set; }

        protected ILogService Log
        {
            get; private set;
        }

        #endregion Properties

        #region Methods

        public abstract IEnumerable<RepositoryAlias> GetAliases();

        public abstract IEnumerable<RepositoryAlias> GetAliases(string criterion);

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

        public void Initialise(IRepositoryContext context)
        {
            Log = context.LogService;
            Initialise();
        }

        protected virtual void Initialise()
        {
            /* By default it does nothing.
             */
        }

        #endregion Methods
    }
}