namespace Probel.Lanceur.Core.Plugins
{
    public interface IMainViewModel
    {
        #region Properties

        object PluginArea { get; set; }
        bool ShowResults { get; set; }

        #endregion Properties
    }
}