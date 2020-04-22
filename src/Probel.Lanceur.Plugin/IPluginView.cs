namespace Probel.Lanceur.Plugin
{
    public interface IMainView
    {
        #region Methods

        void HideControl();

        #endregion Methods
    }

    public interface IPluginView
    {
        #region Methods

        void HidePluginArea();

        void SetPluginArea(object area);

        void ShowPlugin();

        #endregion Methods
    }
}