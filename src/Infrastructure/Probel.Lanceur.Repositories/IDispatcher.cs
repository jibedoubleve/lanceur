using System;

namespace Probel.Lanceur.Repositories
{
    public interface IDispatcher
    {
        #region Methods

        TResult InvokeOnUiThread<TResult>(Func<TResult> action);

        #endregion Methods
    }
}