using Probel.Lanceur.Core.Entities;

namespace Probel.Lanceur.Core.Services.MacroManagement
{
    public interface IMacroAction
    {
        #region Methods

        void Execute(Alias alias);

        IMacroAction With(ICommandRunner cmdrunner, ILogService log, IAliasService aliasService);

        #endregion Methods
    }
}