using System;
using System.Collections.Generic;
using System.Linq;

namespace Probel.Lanceur.Repositories
{
    public class AggregatedRepository : IAliasRepository
    {
        #region Fields

        private const string Message = "Cannot normalise query in a aggregated repository";

        private readonly IEnumerable<IAliasRepository> _repositories;

        #endregion Fields

        #region Constructors

        public AggregatedRepository(IEnumerable<IAliasRepository> repositories)
        {
            _repositories = repositories;

            Keyword = GetKeyword();
        }

        #endregion Constructors

        #region Properties

        public string Keyword { get; }

        #endregion Properties

        #region Methods

        private string GetKeyword()
        {
            var n = new List<string>();

            foreach (var repo in _repositories)
            {
                n.Add(repo.Keyword);
            }

            if (n.Distinct().Count() > 1) { throw new NotSupportedException("A repository aggregate cannot have multiple keywords."); }
            else { return n.ElementAt(0); }
        }

        public IEnumerable<RepositoryAlias> GetAliases()
        {
            var result = new List<RepositoryAlias>();

            foreach (var repo in _repositories)
            {
                result.AddRange(repo.GetAliases());
            }

            return result;
        }

        public IEnumerable<RepositoryAlias> GetAliases(string criterion)
        {
            var result = new List<RepositoryAlias>();

            foreach (var repo in _repositories)
            {
                result.AddRange(repo.GetAliases(criterion));
            }

            return result;
        }

        public void Initialise(IRepositoryContext context)
        {
            foreach (var repo in _repositories)
            {
                repo.Initialise(context);
            }
        }

        public string NormaliseQuery(string query)
        {
            var n = new List<string>();

            foreach (var repo in _repositories)
            {
                n.Add(repo.NormaliseQuery(query));
            }

            if (n.Distinct().Count() > 1) { throw new NotSupportedException("The repositories in the aggregates have different normalisation rules."); }
            else { return n.ElementAt(0); }
        }

        #endregion Methods
    }
}