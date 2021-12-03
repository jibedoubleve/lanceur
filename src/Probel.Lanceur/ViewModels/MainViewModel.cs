using Caliburn.Micro;
using Probel.Lanceur.Actions;
using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Entities.Settings;
using Probel.Lanceur.Core.Helpers;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Images;
using Probel.Lanceur.Plugin;
using Probel.Lanceur.SharedKernel.Logs;
using Probel.Lanceur.SharedKernel.UserCom;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Probel.Lanceur.ViewModels
{
    public class MainViewModel : Screen, IHandle<string>, IMainViewModel
    {
        #region Fields

        private readonly IAliasService _aliasService;
        private readonly IScreenRuler _screenRuler;
        private readonly ISettingsService _settingsService;
        private Query _query;
        private ObservableCollection<object> _aliasNameList;
        private AppSettings _appSettings;
        private string _colour;
        private string _errorMessage;
        private bool _isOnError;
        private double _left;
        private double _opacity;
        private object _selectedResult;
        private string _session;
        private double _top;

        #endregion Fields

        #region Constructors

        public MainViewModel(
            IAliasService aliasService,
            ISettingsService settings,
            IEventAggregator ea,
            IScreenRuler screenRuler,
            ILogService logService,
            IUserNotifyerFactory factory,
            IActionContext ctx
            )
        {
            ActionContext = ctx;
            Notifyer = factory.Get();
            ResultItemHelper.Logger = logService;

            LogService = logService;
            ea.Subscribe(this);

            _screenRuler = screenRuler;
            _settingsService = settings;
            _aliasService = aliasService;
        }

        #endregion Constructors

        #region Properties

        public IActionContext ActionContext { get; internal set; }

        public Query Query
        {
            get => _query;
            set => Set(ref _query, value, nameof(Query));
        }

        public AppSettings AppSettings
        {
            get
            {
                _appSettings = _appSettings ?? _settingsService.Get();
                return _appSettings;
            }
            set => _appSettings = value;
        }

        public string Colour
        {
            get => _colour;
            set => Set(ref _colour, value, nameof(Colour));
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => Set(ref _errorMessage, value);
        }

        public bool IsOnError
        {
            get => _isOnError;
            set => Set(ref _isOnError, value);
        }

        public double Left
        {
            get => _left;
            set => Set(ref _left, value);
        }

        public ILogService LogService { get; }

        public IUserNotifyer Notifyer { get; }

        public double Opacity
        {
            get => _opacity;
            set => Set(ref _opacity, value);
        }

        public ObservableCollection<object> Results
        {
            get => _aliasNameList;
            set => Set(ref _aliasNameList, value, nameof(Results));
        }

        IEnumerable<object> IMainViewModel.Results => Results;

        public object SelectedResult
        {
            get => _selectedResult;
            set => Set(ref _selectedResult, value, nameof(SelectedResult));
        }

        public string Session
        {
            get => _session;
            set => Set(ref _session, value, nameof(Session));
        }

        public double Top
        {
            get => _top;
            set => Set(ref _top, value);
        }

        #endregion Properties

        #region Methods

        private void RefreshAliases()
        {
            Session = _aliasService.GetSession(AppSettings.SessionId);
            var aliases = _aliasService.GetAliasNames(AppSettings.SessionId, null);
            var r = aliases.GetRefreshed();
            Results = new ObservableCollection<object>(r);
        }

        public ExecutionResult Execute(string cmdline)
        {
            Query result;
            if (SelectedResult is Query query)
            {
                query.SetParameters(cmdline);
                result = query;
            }
            else { result = Query.FromText(cmdline); }

            return Execute(result);
        }
        public ExecutionResult Execute(Query query)
        {
            try
            {
                var sid = _settingsService.Get().SessionId;
                var r = _aliasService.Execute(query, sid);

                if (r.IsError) { ErrorMessage = r.Error; }

                return r;
            }
            catch (Exception ex)
            {
                /* I swallow the error as this crash shouldn't crash the application
                 * I log and continue without any other warning.
                 */
                var msg = $"An error occured while trying to execute the alias '{query?.Name ?? "..."}' with params '{query?.Parameters ?? "..."}'";
                LogService.Error(msg, ex);
                ErrorMessage = $"Error: {ex.Message}";
                return ExecutionResult.Failure(msg);
            }
        }

        public void Handle(string message)
        {
            if (message == Services.UiEvent.CornerCommand)
            {
                var r = _screenRuler.StickTo(Left, Top);
                Left = r.X;
                Top = r.Y;
                SaveSettings();
            }
            if (message == Services.UiEvent.CenterCommand)
            {
                var r = _screenRuler.Center(150);
                Left = r.X;
                Top = r.Y;
                SaveSettings();
            }
        }

        public void LoadAliases() => RefreshAliases();

        public void LoadSettings()
        {
            var s = _settingsService.Get().WindowSection;
            var pos = s.Position;
            Top = pos.Top;
            Left = pos.Left;
            Colour = s.Colour;
            Opacity = s.Opacity;
        }

        public void OnShow()
        {
            // In any case, I don't want to see an hourglass cursor
            // indicating a work is going on when I'm displaying
            // this view as I just want to launch a shortcut!
            Notifyer.NotifyEndWait();

            AppSettings = _settingsService.Get();
            RefreshAliases();
        }

        public void RefreshAliases(string criterion)
        {
            Session = _aliasService.GetSession(AppSettings.SessionId);
            var l = _aliasService.GetAliasNames(AppSettings.SessionId, criterion);
            Results = new ObservableCollection<object>(l.GetRefreshed());
        }

        public void SaveSettings()
        {
            AppSettings.WindowSection.Position.Left = Left;
            AppSettings.WindowSection.Position.Top = Top;

            _settingsService.SavePosition(AppSettings);
            AppSettings = _settingsService.Get();
        }

        public void SetResult(IEnumerable<object> source, bool keepalive = false)
        {
            Results = new ObservableCollection<object>(source);
        }

        #endregion Methods
    }
}