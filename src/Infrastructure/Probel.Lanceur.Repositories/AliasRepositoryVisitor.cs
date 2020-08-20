using System.Linq;

namespace Probel.Lanceur.Repositories
{
    internal class AliasRepositoryVisitor
    {
        #region Fields

        private readonly IAliasRepository _repository;

        #endregion Fields

        #region Constructors

        public AliasRepositoryVisitor(IAliasRepository repository)
        {
            _repository = repository;
        }

        #endregion Constructors

        #region Methods

        public bool TrySetKeyword(string keyword)
        {
            var s = (from p in _repository.GetType().GetProperties()
                     where p.CanWrite
                        && p.Name.ToLower() == "keyword"
                     select p).FirstOrDefault();

            if (s != null)
            {
                s.SetValue(_repository, keyword);
                return true;
            }
            else { return false; }
        }

        #endregion Methods
    }
}