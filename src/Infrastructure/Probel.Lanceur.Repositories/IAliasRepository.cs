using System.Collections.Generic;

namespace Probel.Lanceur.Repositories
{
    public interface IAliasRepository
    {
        #region Properties

        string Keyword { get; }

        #endregion Properties

        #region Methods

        IEnumerable<RepositoryAlias> GetAliases();

        IEnumerable<RepositoryAlias> GetAliases(string criterion);

        /// <summary>
        /// Removes from the query the keyword of the repository.
        /// In other words, if you have a repository with ':' as a
        /// keyword, it'll remove any starting ':' from the query.
        /// </summary>
        /// <param name="query">The query to execute</param>
        /// <returns></returns>
        string NormaliseQuery(string query);

        void Initialise(IRepositoryContext context);

        #endregion Methods
    }
}