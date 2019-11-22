using Probel.Lanceur.Core.Entities;

namespace Probel.Lanceur.Core.Services
{
    public interface ICommandRunner
    {
        #region Methods

        void Run(Shortcut shortcut);

        #endregion Methods
    }
}