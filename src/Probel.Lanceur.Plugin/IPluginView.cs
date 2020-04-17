namespace Probel.Lanceur.Plugin
{
    public interface IPluginView
    {
        #region Methods

        void HideResults();

        void SetPluginArea(object area);

        void ShowResults();

        #endregion Methods
    }
}