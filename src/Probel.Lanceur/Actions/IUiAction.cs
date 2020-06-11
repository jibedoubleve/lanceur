using Probel.Lanceur.Core.Services;

namespace Probel.Lanceur.Actions
{
    public interface IUiAction
    {
        #region Methods

        ExecutionResult Execute(string arg);

        IUiAction With(IActionContext context);

        #endregion Methods
    }
}