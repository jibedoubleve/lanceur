using Caliburn.Micro;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Helpers;
using Probel.Lanceur.Models;
using Probel.Lanceur.Models.Settings;
using Probel.Lanceur.Services;
using System.Collections.ObjectModel;

namespace Probel.Lanceur.ViewModels
{
    public class SettingsViewModel : Screen
    {
        #region Fields

        private readonly IDatabaseService _databaseService;

        private readonly ISettingsService _settingsService;

        private AppSettingsModel _appSettings;

        private AliasSessionModel _currentSession;
        private ObservableCollection<AliasSessionModel> _sessons;

        #endregion Fields

        #region Constructors

        public SettingsViewModel(ISettingsService settingsService, IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            _settingsService = settingsService;
        }

        #endregion Constructors

        #region Properties

        public AppSettingsModel AppSettings
        {
            get => _appSettings;
            set => Set(ref _appSettings, value, nameof(AppSettings));
        }

        public AliasSessionModel CurrentSession
        {
            get => _currentSession;
            set => Set(ref _currentSession, value, nameof(CurrentSession));
        }

        public ObservableCollection<AliasSessionModel> Sessions
        {
            get => _sessons;
            set => Set(ref _sessons, value, nameof(Sessions));
        }

        #endregion Properties

        #region Methods

        public void RefreshData()
        {
            var appSettings = _settingsService.Get().AsModel();
            AppSettings = appSettings;

            var sessions = _databaseService.GetSessions().AsModel();
            Sessions = new ObservableCollection<AliasSessionModel>(sessions);

            var curSession = sessions.GetSession(appSettings.SessionId);
            CurrentSession = curSession;
        }

        public void ResetSettings()
        {
            var pos = AppSettings.WindowSection.Position;
            AppSettings = new AppSettingsModel();
            AppSettings.WindowSection.Position = pos;
            AppSettings.SessionId = CurrentSession?.Id ?? 1;
        }

        public void SaveSettings()
        {
            AppSettings.SessionId = CurrentSession?.Id ?? 1;
            _settingsService.Save(AppSettings.AsEntity());
        }

        #endregion Methods
    }
}