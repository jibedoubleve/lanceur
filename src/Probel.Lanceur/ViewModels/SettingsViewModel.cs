using Caliburn.Micro;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Helpers;
using Probel.Lanceur.Infrastructure;
using Probel.Lanceur.Models;
using Probel.Lanceur.Models.Settings;
using Probel.Lanceur.Plugin;
using Probel.Lanceur.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Probel.Lanceur.ViewModels
{
    public enum NotificationTypes
    {
        Classic,
        Win10,
    }

    public class SettingsViewModel : Screen
    {
        #region Fields

        private readonly IDataSourceService _databaseService;
        private readonly IAppRestarter _restarter;
        private readonly ISettingsService _settingsService;
        private readonly IUserNotifyer _userNotifyer;
        private AppSettingsModel _appSettings;

        private string _colour;
        private AliasSessionModel _currentSession;
        private string _databasePath;
        private bool _isRebootNeeded;
        private ObservableCollection<AliasSessionModel> _sessions;

        #endregion Fields

        #region Constructors

        public SettingsViewModel(ISettingsService settingsService,
            IDataSourceService databaseService,
            IUserNotifyerFactory factory,
            IAppRestarter restarter)
        {
            _restarter = restarter;
            _userNotifyer = factory.Get();
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

        public string Colour
        {
            get => _colour;
            set
            {
                if (Set(ref _colour, value, nameof(Colour)))
                {
                    _appSettings.WindowSection.Colour = value;
                }
            }
        }

        public AliasSessionModel CurrentSession
        {
            get => _currentSession;
            set => Set(ref _currentSession, value, nameof(CurrentSession));
        }

        public string DatabasePath
        {
            get => _databasePath;
            set
            {
                if (Set(ref _databasePath, value, nameof(DatabasePath)))
                {
                    AppSettings.DatabasePath = _databasePath;
                }
            }
        }

        public bool IsRebootNeeded
        {
            get => _isRebootNeeded;
            set => Set(ref _isRebootNeeded, value, nameof(NeedReboot));
        }

        public ObservableCollection<AliasSessionModel> Sessions
        {
            get => _sessions;
            set => Set(ref _sessions, value, nameof(Sessions));
        }

        #endregion Properties

        #region Methods

        private bool NeedReboot()
        {
            var sr = _settingsService.Get();
            var res = sr.DatabasePath != AppSettings.DatabasePath;
            res |= sr.WindowSection.NotificationType != AppSettings.WindowSection.NotificationType;
            return res;
        }

        public void RefreshData()
        {
            var appSettings = _settingsService.Get().AsModel();
            AppSettings = appSettings;
            Colour = AppSettings.WindowSection.Colour;

            DatabasePath = AppSettings.DatabasePath;

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

        public async Task SaveSettings()
        {
            AppSettings.SessionId = CurrentSession?.Id ?? 1;

            var e = AppSettings.AsEntity();
            _settingsService.Save(e);

            _userNotifyer.NotifyInfo("Settings has been saved.");

            if (NeedReboot())
            {
                IsRebootNeeded = true;
                if (await _restarter.DoRestartAsync()) { _restarter.Restart(); }
            }
        }

        #endregion Methods
    }
}