namespace Probel.Lanceur.Core.Entities.Settings
{
    public class AppSettings
    {
        #region Fields

        private string _notificationType = "default";

        #endregion Fields

        #region Properties

        public string DatabasePath { get; set; } = @"%appdata%\probel\Lanceur\data.db";
        public HotKeySettings HotKey { get; set; } = new HotKeySettings();
        public RepositorySettings RepositorySection { get; set; } = new RepositorySettings();
        public long SessionId { get; set; } = 1;
        public WindowSettings WindowSection { get; set; } = new WindowSettings();

        #endregion Properties
    }
}