namespace Probel.Lanceur.Plugin
{
    /// <summary>
    /// Retrieve the view used for the plugin.
    /// </summary>
    public interface IPluginViewAdapter
    {
        #region Methods

        IPluginView GetView();

        #endregion Methods
    }
}