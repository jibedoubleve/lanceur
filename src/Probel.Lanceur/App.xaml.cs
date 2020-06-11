using Probel.Lanceur.Helpers;
using System.Windows;

namespace Probel.Lanceur
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Methods

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            SingleInstance.ReleaseMutex();
        }

        #endregion Methods
    }
}