using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Plugin;

namespace Probel.Lanceur.Core.Services.MacroManagement
{
    public interface IMacroAction
    {
        #region Methods

        void Execute(Alias alias);

        IMacroAction With(ILogService log, ICommandRunner runner);

        #endregion Methods
    }
}