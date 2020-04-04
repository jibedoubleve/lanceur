namespace Probel.Lanceur.Core.Entities.Settings
{
    public class AppSettings
    {
        #region Properties

        public HotKeySettings HotKey { get; set; } = new HotKeySettings();
        public long SessionId { get; set; } = 1;
        public WindowSettings WindowSection { get; set; } = new WindowSettings();
        public DatabaseSettings DatabaseSection { get; set; } = new DatabaseSettings();
        #endregion Properties
    }
}