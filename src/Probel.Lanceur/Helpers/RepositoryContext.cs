using Probel.Lanceur.Infrastructure;
using Probel.Lanceur.Repositories;

namespace Probel.Lanceur.Helpers
{
    public class RepositoryContext : IRepositoryContext
    {
        #region Constructors

        public RepositoryContext(ILogService logService, IDispatcher dispatcher)
        {
            LogService = logService;
            Dispatcher = dispatcher;
        }

        #endregion Constructors

        #region Properties

        public IDispatcher Dispatcher { get; }
        public ILogService LogService { get; }

        #endregion Properties
    }
}