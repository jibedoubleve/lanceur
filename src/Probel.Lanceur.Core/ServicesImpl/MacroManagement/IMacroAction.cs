using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;

namespace Probel.Lanceur.Core.ServicesImpl.MacroManagement
{
    public interface IMacroAction
    {
        #region Methods

        void Execute(Alias alias);

        IMacroAction With(ICommandRunner cmdrunner, ILogService log, IAliasService aliasService);

        #endregion Methods
    }
}