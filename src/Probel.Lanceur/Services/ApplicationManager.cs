using Probel.Lanceur.Core;
using Probel.Lanceur.Core.Plugins;
using System;
using System.Windows;

namespace Probel.Lanceur.Services
{
    public class ApplicationManager : IApplicationManager
    {
        #region Methods

        public IMainView GetMain()
        {
            var mw = Application.Current.MainWindow;
            return mw as IMainView ?? throw new NotSupportedException($"The main view is of type '{mw.GetType()}' doesn't implement '{typeof(IMainView)}'");
        }

        #endregion Methods
    }
}