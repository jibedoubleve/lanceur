using Caliburn.Micro;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Helpers;
using Probel.Lanceur.Models;
using Probel.Lanceur.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Probel.Lanceur.ViewModels
{
    public class EditSessionViewModel : Screen
    {
        #region Fields

        private readonly IDatabaseService _databaseService;
        private ShortcutSessionModel _currentSession;
        private ObservableCollection<ShortcutSessionModel> _sessions;

        private ObservableCollection<ShortcutModel> _shortcuts;

        #endregion Fields

        #region Constructors

        public EditSessionViewModel(IDatabaseService databaseService, ISettingsService settingsService)
        {
            _settingsService = settingsService;
            _databaseService = databaseService;
        }

        #endregion Constructors

        #region Properties

        public ISettingsService _settingsService { get; }

        public ShortcutSessionModel CurrentSession
        {
            get => _currentSession;
            set => Set(ref _currentSession, value, nameof(CurrentSession));
        }

        public ObservableCollection<ShortcutSessionModel> Sessions
        {
            get => _sessions;
            set => Set(ref _sessions, value, nameof(Sessions));
        }

        public ObservableCollection<ShortcutModel> Shortcuts
        {
            get => _shortcuts;
            set => Set(ref _shortcuts, value, nameof(Shortcuts));
        }

        #endregion Properties

        #region Methods

        public void DeleteName() => _databaseService.Delete(CurrentSession.AsEntity());

        public void RefreshData()
        {
            var stg = _settingsService.Get();
            var sessions = _databaseService.GetSessions().AsModel();
            Sessions = new ObservableCollection<ShortcutSessionModel>(sessions);
            CurrentSession = Sessions.GetSession(stg.SessionId);

            RefreshShortcuts();
        }

        public void RefreshShortcuts()
        {
            if (CurrentSession != null)
            {
                var sessionId = CurrentSession.Id;
                var shortcuts = _databaseService.GetShortcuts(sessionId).AsModel();
                Shortcuts = new ObservableCollection<ShortcutModel>(shortcuts);
            }
        }

        public void UpdateName() => _databaseService.Update(CurrentSession.AsEntity());

        #endregion Methods
    }
}