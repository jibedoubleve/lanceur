using System;
using System.Collections.Generic;

namespace Probel.Lanceur.Repositories
{
    internal class AliasRepositoryCollection : IAliasRepository
    {
        #region Fields

        private readonly List<IAliasRepository> _collection = new List<IAliasRepository>();

        #endregion Fields

        #region Properties

        public IEnumerable<IAliasRepository> Items => _collection;

        /// <summary>
        /// This is not used.
        /// </summary>
        //TODO: refactoring needed to remove this.
        public string Keyword => throw new System.NotImplementedException();

        #endregion Properties

        #region Methods

        public void Add(IAliasRepository item) => _collection.Add(item);

        public IEnumerable<RepositoryAlias> GetAliases()
        {
            var result = new List<RepositoryAlias>();

            foreach (var item in _collection)
            {
                result.AddRange(item.GetAliases());
            }

            return result;
        }

        public IEnumerable<RepositoryAlias> GetAliases(string criterion)
        {
            var result = new List<RepositoryAlias>();

            foreach (var item in _collection)
            {
                result.AddRange(item.GetAliases(criterion));
            }

            return result;
        }

        public string NormaliseQuery(string query)
        {
            foreach (var repo in _collection)
            {
                query = repo.NormaliseQuery(query.ToLower()).Trim();
            }
            return query;
        }

        [Obsolete("Does nothing, can be removed")]
        public void Initialise(IRepositoryContext context)
        {
        }

        public void Merge(AliasRepositoryCollection collection)
        {
            _collection.AddRange(collection.Items);
        }

        public IEnumerable<IAliasRepository> ToList() => _collection;

        #endregion Methods
    }
}