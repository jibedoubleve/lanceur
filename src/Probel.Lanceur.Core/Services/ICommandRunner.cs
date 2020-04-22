using Probel.Lanceur.Core.Entities;

namespace Probel.Lanceur.Core.Services
{
    public interface ICommandRunner
    {
        #region Methods

        ExecutionResult Execute(Alias alias);

        #endregion Methods
    }
}