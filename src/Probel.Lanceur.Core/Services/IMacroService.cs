using Probel.Lanceur.Core.Entities;

namespace Probel.Lanceur.Core.Services
{
    public interface IMacroService
    {
        #region Methods

        void Handle(Alias cmd);

        bool Has(string name);

        IMacroService With(ICommandRunner cmdrunner, IAliasService aliasService);

        #endregion Methods
    }
}