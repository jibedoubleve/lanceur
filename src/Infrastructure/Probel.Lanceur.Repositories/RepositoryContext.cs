using Probel.Lanceur.SharedKernel.Logs;

namespace Probel.Lanceur.Repositories
{
    public class RepositoryContext : IRepositoryContext
    {
        #region Constructors

        public RepositoryContext(ILogService logService, IRepositorySettings repositorySettings)
        {
            LogService = logService;
            Settings = repositorySettings;
        }

        #endregion Constructors

        #region Properties

        public ILogService LogService { get; }

        public IRepositorySettings Settings { get; set; }

        #endregion Properties
    }
}