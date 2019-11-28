using Probel.Lanceur.ViewModels;
using System;

namespace Probel.Lanceur.Helpers
{
#pragma warning disable CA1063 // Implement IDisposable Correctly

    public class DeactivateHotKey : IDisposable
    {
        #region Constructors

        private DeactivateHotKey()
        {
            SetupViewModel.IsBusy = true;
        }

        #endregion Constructors

        #region Methods

        public static DeactivateHotKey During() => new DeactivateHotKey();

        public void Dispose() => SetupViewModel.IsBusy = false;

        #endregion Methods
    }

#pragma warning restore CA1063 // Implement IDisposable Correctly
}