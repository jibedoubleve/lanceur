namespace Probel.Lanceur.Core.Entities.Settings
{
    public class AppSettings
    {
        #region Properties

#if DEBUG
        public string DatabasePath
        {
            get => @"%appdata%\probel\Lanceur\debug_data.db";
            set { }
        }
#else
        public string DatabasePath { get; set; } = @"%appdata%\probel\Lanceur\data.db";
#endif
        public HotKeySettings HotKey { get; set; } = new HotKeySettings();
        public long SessionId { get; set; } = 1;
        public WindowSettings WindowSection { get; set; } = new WindowSettings();

        #endregion Properties
    }
}