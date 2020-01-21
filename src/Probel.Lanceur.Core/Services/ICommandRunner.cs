using Probel.Lanceur.Core.Entities;

namespace Probel.Lanceur.Core.Services
{
    public interface ICommandRunner
    {
        #region Methods

        bool Run(Alias alias);

        #endregion Methods
    }
}