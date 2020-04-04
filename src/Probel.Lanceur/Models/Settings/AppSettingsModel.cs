using Caliburn.Micro;

namespace Probel.Lanceur.Models.Settings
{
    public class AppSettingsModel : PropertyChangedBase
    {
        #region Fields

        private DatabaseSettingsModel _databaseSection;
        private HotKeySettingsModel _hotKey;
        private long _sessionId;
        private WindowSettingsModel _windowSection;

        #endregion Fields

        #region Constructors

        public AppSettingsModel()
        {
            WindowSection = new WindowSettingsModel();
            HotKey = new HotKeySettingsModel();
        }

        #endregion Constructors

        #region Properties

        public DatabaseSettingsModel DatabaseSection
        {
            get => _databaseSection;
            set => Set(ref _databaseSection, value, nameof(DatabaseSection));
        }

        public HotKeySettingsModel HotKey
        {
            get => _hotKey;
            set => Set(ref _hotKey, value, nameof(HotKey));
        }

        public long SessionId
        {
            get => _sessionId;
            set => Set(ref _sessionId, value, nameof(SessionId));
        }

        public WindowSettingsModel WindowSection
        {
            get => _windowSection;
            set => Set(ref _windowSection, value, nameof(WindowSection));
        }

        #endregion Properties
    }
}