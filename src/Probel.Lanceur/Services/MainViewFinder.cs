using Probel.Lanceur.Plugin;
using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Probel.Lanceur.Services
{
    /// <summary>
    /// This adapter is meant to get the View used for a plugin.
    /// </summary>
    public class MainViewFinder : IPluginViewFinder, IMainViewFinder
    {
        #region Methods

        public IMainView GetMainView() => GetView<IMainView>();

        /// <summary>
        /// This method retrieve the <see cref="Application.Current.MainWindow"/> and
        /// try to cast it into a <see cref="IPluginView"/>. In case it failed an
        /// exception is thrown
        /// </summary>
        /// <returns>The <see cref="IPluginView"/> used in the plugin</returns>
        /// <exception cref="NotSupportedException">In case the View cannot be casted into
        /// a <see cref="IPluginView"/></exception>
        public IPluginView GetView() => GetView<IPluginView>();

        public IMainViewModel GetViewModel() => GetViewModel<IMainViewModel>();

        private T GetView<T>([CallerMemberName]string sender = null) where T : class
        {
            var mv = Application.Current.MainWindow;
            return mv as T ?? throw new NotSupportedException($"[{sender}] Cannot cast View'{mv.GetType()}' into '{typeof(T)}'");
        }

        private T GetViewModel<T>([CallerMemberName]string sender = null) where T : class
        {
            var mv = Application.Current.MainWindow;
            return mv.DataContext as T ?? throw new NotSupportedException($"[{sender}] Cannot cast ViewModel '{mv.GetType()}' into '{typeof(T)}'");
        }

        #endregion Methods
    }
}