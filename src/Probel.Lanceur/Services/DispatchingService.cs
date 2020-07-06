using Probel.Lanceur.Repositories;
using System;
using System.Diagnostics;
using System.Windows;

namespace Probel.Lanceur.Services
{
    public class DispatchingService : IDispatcher
    {
        #region Methods

        public TResult InvokeOnUiThread<TResult>(Func<TResult> action)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var r = action();
                Trace.WriteLine(r);
            });
            return default;
        }

        #endregion Methods
    }
}