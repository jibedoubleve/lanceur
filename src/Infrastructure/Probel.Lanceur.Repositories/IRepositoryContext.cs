using Probel.Lanceur.SharedKernel.Logs;

namespace Probel.Lanceur.Repositories
{
    public interface IRepositoryContext
    {
        #region Properties

        ILogService LogService { get; }

        IRepositorySettings Settings { get; }

        #endregion Properties
    }
}