using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Plugin;

namespace Probel.Lanceur.Core.ServicesImpl.MacroManagement
{
    public interface IMacroAction
    {
        #region Methods

        void Execute(Alias alias);

        IMacroAction With(ILogService log, ICommandRunner runner);

        #endregion Methods
    }
}