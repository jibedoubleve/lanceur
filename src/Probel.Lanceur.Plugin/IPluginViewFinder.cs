namespace Probel.Lanceur.Plugin
{
    /// <summary>
    /// Meant to be used for the internal alias and macros
    /// </summary>
    public interface IMainViewFinder
    {
        #region Methods

        IMainView GetMainView();

        IMainViewModel GetViewModel();

        #endregion Methods
    }

    /// <summary>
    /// Retrieve the view used for the plugin.
    /// </summary>
    public interface IPluginViewFinder
    {
        #region Methods

        IPluginView GetView();

        #endregion Methods
    }
}