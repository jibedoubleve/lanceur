namespace Probel.Lanceur.Core.Plugins
{
    public interface IMainView
    {
        #region Methods

        void HideResults();

        void SetPluginArea(object area);

        void ShowResults();

        #endregion Methods
    }
}