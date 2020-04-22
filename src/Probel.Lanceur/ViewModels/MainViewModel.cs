using Caliburn.Micro;
using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Entities.Settings;
using Probel.Lanceur.Core.Helpers;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Plugin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Probel.Lanceur.ViewModels
{
    public class MainViewModel : Screen, IHandle<string>, IMainViewModel
    {
        #region Fields

        private readonly IAliasService _aliasService;
        private readonly IParameterResolver _resolver;
        private readonly IScreenRuler _screenRuler;
        private readonly ISettingsService _settingsService;
        private AliasText _aliasName;
        private ObservableCollection<object> _aliasNameList;
        private AppSettings _appSettings;
        private string _colour;
        private bool _isOnError;
        private double _left;
        private double _opacity;
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
            IParameterResolver resolver,
            IUserNotifyer notifyer
            )
        {
            Notifyer = notifyer;

            LogService = logService;
            ea.Subscribe(this);

            _resolver = resolver;
            _screenRuler = screenRuler;
            _settingsService = settings;
            _aliasService = aliasService;
        }

        #endregion Constructors

        #region Properties

        public AliasText AliasName
        {
            get => _aliasName;
            set => Set(ref _aliasName, value, nameof(AliasName));
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

        public bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
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

        /// <summary>
        /// Executes the alias and returns <c>True</c> if execution was a success.
        /// Otherwise returns <c>False</c>
        /// </summary>
        /// <param name="cmdLine">The command line (the alias & the arguments) to be executed.</param>
        /// <returns><c>True</c> on success; otherwise <c>False</c></returns>
        public ExecutionResult ExecuteText(string cmdLine)
        {
            try
            {
                var sid = _settingsService.Get().SessionId;
                return _aliasService.Execute(cmdLine, sid);
            }
            catch (Exception ex)
            {
                /* I swallow the error as this crash shouldn't crash the application
                 * I log and continue without any other warning.
                 */
                LogService.Error($"An error occured while trying to execute the alias '{cmdLine}'", ex);
                return ExecutionResult.Failure;
            }
        }

        public ExecutionResult ExecuteText(string cmdline1, string cmdline2)
        {
            var sid = _settingsService.Get().SessionId;
            var cmd = _resolver.Merge(cmdline1, cmdline2, sid);

            return ExecuteText(cmd.ToString());
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
            Results = new ObservableCollection<object>(l);
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

        private void RefreshAliases()
        {
            Session = _aliasService.GetSession(AppSettings.SessionId);
            var l = _aliasService.GetAliasNames(AppSettings.SessionId);
            Results = new ObservableCollection<object>(l);
        }

        #endregion Methods
    }
}