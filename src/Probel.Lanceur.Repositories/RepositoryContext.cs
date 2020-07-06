using Probel.Lanceur.Infrastructure;

namespace Probel.Lanceur.Repositories
{
    public class RepositoryContext : IRepositoryContext
    {
        #region Constructors

        public RepositoryContext(ILogService logService)
        {
            LogService = logService;
        }

        #endregion Constructors

        #region Properties

        public ILogService LogService { get; }

        #endregion Properties
    }
}