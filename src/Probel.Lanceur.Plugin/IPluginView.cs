namespace Probel.Lanceur.Plugin
{
    public interface IPluginView
    {
        #region Methods

        void HidePlugin();

        void SetPluginArea(object area);

        void ShowPlugin();

        #endregion Methods
    }
}