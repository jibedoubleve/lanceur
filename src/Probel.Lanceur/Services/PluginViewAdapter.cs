using Probel.Lanceur.Plugin;
using System;
using System.Windows;

namespace Probel.Lanceur.Services
{
    /// <summary>
    /// This adapter is meant to get the View used for a plugin.
    /// </summary>
    public class PluginViewAdapter : IPluginViewAdapter
    {
        #region Methods

        /// <summary>
        /// This method retrieve the <see cref="Application.Current.MainWindow"/> and
        /// try to cast it into a <see cref="IPluginView"/>. In case it failed an
        /// exception is thrown
        /// </summary>
        /// <returns>The <see cref="IPluginView"/> used in the plugin</returns>
        /// <exception cref="NotSupportedException">In case the View cannot be casted into
        /// a <see cref="IPluginView"/></exception>
        public IPluginView GetView()
        {
            var mw = Application.Current.MainWindow;
            return mw as IPluginView ?? throw new NotSupportedException($"The main view is of type '{mw.GetType()}' doesn't implement '{typeof(IPluginView)}'");
        }

        #endregion Methods
    }
}