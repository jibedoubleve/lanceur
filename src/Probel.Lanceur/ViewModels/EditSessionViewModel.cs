using Caliburn.Micro;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Helpers;
using Probel.Lanceur.Models;
using Probel.Lanceur.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Probel.Lanceur.ViewModels
{
    public class EditSessionViewModel : Screen
    {
        #region Fields

        private readonly IDataSourceService _databaseService;
        private readonly IUserNotifyer _notifyer;
        private ObservableCollection<AliasModel> _aliases;
        private object _currentSession;
        private bool _isCreatingNewSession;
        private ObservableCollection<object> _sessions;

        #endregion Fields

        #region Constructors

        public EditSessionViewModel(IDataSourceService databaseService, ISettingsService settingsService, IUserNotifyer notifyer)
        {
            _notifyer = notifyer;
            _settingsService = settingsService;
            _databaseService = databaseService;
        }

        #endregion Constructors

        #region Properties

        public ISettingsService _settingsService { get; }

        public ObservableCollection<AliasModel> Aliases
        {
            get => _aliases;
            set => Set(ref _aliases, value, nameof(Aliases));
        }

        public object CurrentSession
        {
            get => _currentSession;
            set
            {
                if (Set(ref _currentSession, value, nameof(CurrentSession)))
                {
                    if (value is NewAliasSessionModel ns)
                    {
                        IsCreatingNewSession = true;
                        ns.Reset();
                    }
                    else { IsCreatingNewSession = false; }
                }
            }
        }

        public bool IsCreatingNewSession
        {
            get => _isCreatingNewSession;
            set => Set(ref _isCreatingNewSession, value, nameof(IsCreatingNewSession));
        }

        public ObservableCollection<object> Sessions
        {
            get => _sessions;
            set => Set(ref _sessions, value, nameof(Sessions));
        }

        private AliasSessionModel CastedCurrentSession => (CurrentSession as AliasSessionModel) ?? new AliasSessionModel();

        #endregion Properties

        #region Methods

        public void DeleteName()
        {
            if (CurrentSession != null)
            {
                var name = (CurrentSession as AliasSessionModel)?.Name ?? throw new NullReferenceException("The current session to be deleted is null");
                if (_notifyer.Ask($"Do you want to delete the session '{name}'?") == NotificationResult.Affirmative)
                {

                    _databaseService.Delete(CastedCurrentSession.AsEntity());

                    var toDel = (from s in Sessions
                                 where s is AliasSessionModel m
                                    && m.Id == CastedCurrentSession.Id
                                 select s).SingleOrDefault();

                    if (toDel != null) { Sessions.Remove(toDel); }
                }
            }
        }

        public void RefreshAliases()
        {
            _notifyer.NotifyWait();
            if (CurrentSession != null)
            {
                var sessionId = CastedCurrentSession.Id;
                var aliases = _databaseService.GetAliases(sessionId).AsModel();
                Aliases = new ObservableCollection<AliasModel>(aliases);
            }
            _notifyer.NotifyEndWait();
        }

        public void RefreshData()
        {
            var stg = _settingsService.Get();
            var sessions = _databaseService.GetSessions().AsModel();
            Sessions = new ObservableCollection<object>(sessions);
            Sessions.Add(new NewAliasSessionModel());

            CurrentSession = (from s in Sessions
                              where s is AliasSessionModel m
                              && m.Id == stg.SessionId
                              select s as AliasSessionModel).FirstOrDefault();

            RefreshAliases();
        }

        public void UpdateName()
        {
            if (CurrentSession is NewAliasSessionModel)
            {
                _databaseService.Create(CastedCurrentSession?.AsEntity());
                RefreshData();
            }
            else if (CurrentSession is AliasSessionModel)
            {
                _databaseService.Update(CastedCurrentSession.AsEntity());
            }
        }

        #endregion Methods
    }
}