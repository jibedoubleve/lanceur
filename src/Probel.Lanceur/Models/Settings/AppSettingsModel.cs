using Caliburn.Micro;

namespace Probel.Lanceur.Models.Settings
{
    public class AppSettingsModel : PropertyChangedBase
    {
        #region Fields

        private string _databasePath;
        private HotKeySettingsModel _hotKey;
        private RepositorySettingsModel _repositorySection;
        private long _sessionId;
        private WindowSettingsModel _windowSection;

        #endregion Fields

        #region Constructors

        public AppSettingsModel()
        {
            WindowSection = new WindowSettingsModel();
            HotKey = new HotKeySettingsModel();
            RepositorySection = new RepositorySettingsModel();
        }

        #endregion Constructors

        #region Properties

        public string DatabasePath
        {
            get => _databasePath;
            set => Set(ref _databasePath, value);
        }

        public HotKeySettingsModel HotKey
        {
            get => _hotKey;
            set => Set(ref _hotKey, value);
        }

        public RepositorySettingsModel RepositorySection
        {
            get => _repositorySection;
            set => Set(ref _repositorySection, value);
        }

        public long SessionId
        {
            get => _sessionId;
            set => Set(ref _sessionId, value);
        }

        public WindowSettingsModel WindowSection
        {
            get => _windowSection;
            set => Set(ref _windowSection, value);
        }

        #endregion Properties
    }
}