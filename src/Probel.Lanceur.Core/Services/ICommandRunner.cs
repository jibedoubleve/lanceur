using Probel.Lanceur.Core.Entities;

namespace Probel.Lanceur.Core.Services
{
    public interface ICommandRunner
    {
        #region Methods

        bool Execute(Alias alias);

        #endregion Methods
    }
}