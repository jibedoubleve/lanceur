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
        private AliasSessionModel _currentSession;
        private ObservableCollection<AliasSessionModel> _sessions;

        private ObservableCollection<AliasModel> _aliases;

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

        public AliasSessionModel CurrentSession
        {
            get => _currentSession;
            set => Set(ref _currentSession, value, nameof(CurrentSession));
        }

        public ObservableCollection<AliasSessionModel> Sessions
        {
            get => _sessions;
            set => Set(ref _sessions, value, nameof(Sessions));
        }

        public ObservableCollection<AliasModel> Aliases
        {
            get => _aliases;
            set => Set(ref _aliases, value, nameof(Aliases));
        }

        #endregion Properties

        #region Methods

        public void DeleteName() => _databaseService.Delete(CurrentSession.AsEntity());

        public void RefreshData()
        {
            var stg = _settingsService.Get();
            var sessions = _databaseService.GetSessions().AsModel();
            Sessions = new ObservableCollection<AliasSessionModel>(sessions);
            CurrentSession = Sessions.GetSession(stg.SessionId);

            RefreshAliases();
        }

        public void RefreshAliases()
        {
            if (CurrentSession != null)
            {
                var sessionId = CurrentSession.Id;
                var aliases = _databaseService.GetAliases(sessionId).AsModel();
                Aliases = new ObservableCollection<AliasModel>(aliases);
            }
        }

        public void UpdateName() => _databaseService.Update(CurrentSession.AsEntity());

        #endregion Methods
    }
}