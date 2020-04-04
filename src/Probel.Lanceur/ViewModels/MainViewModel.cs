using Caliburn.Micro;
using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Entities.Settings;
using Probel.Lanceur.Core.Helpers;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Probel.Lanceur.ViewModels
{
    public class MainViewModel : Screen, IHandle<string>
    {
        #region Fields

        private readonly IAliasService _aliasService;
        private readonly IParameterResolver _resolver;
        private readonly IScreenRuler _screenRuler;
        private readonly ISettingsService _settingsService;
        private AliasText _aliasName;
        private ObservableCollection<AliasText> _aliasNameList;
        private AppSettings _appSettings;
        private string _colour;
        private bool _isOnError;
        private double _left;
        private double _opacity;
        private double _top;

        #endregion Fields

        #region Constructors

        public MainViewModel(
            IAliasService aliasService,
            ISettingsService settings,
            IEventAggregator ea,
            IScreenRuler screenRuler,
            ILogService logService,
            IParameterResolver resolver
            )
        {
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

        public ObservableCollection<AliasText> AliasNameList
        {
            get => _aliasNameList;
            set => Set(ref _aliasNameList, value, nameof(AliasNameList));
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
#if DEBUG
            get
            {
                _colour = "Crimson";
                return _colour;
            }
            set { }
#else
            get => _colour;
            set => Set(ref _colour, value, nameof(Colour));
#endif
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

        public double Opacity
        {
            get => _opacity;
            set => Set(ref _opacity, value);
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
        public async Task<ExecutionResult> ExecuteTextAsync(string cmdLine)
        {
            try { return _aliasService.Execute(cmdLine); }
            catch (Exception ex)
            {
                /* I swallow the error as this crash shouldn't crash the application
                 * I log and continue without any other warning.
                 */
                LogService.Error($"An error occured while trying to execute the alias '{cmdLine}'", ex);
                return ExecutionResult.Failure;
            }
        }

        public async Task<ExecutionResult> ExecuteTextAsync(string cmdline1, string cmdline2)
        {
            var cmd = _resolver.Merge(cmdline1, cmdline2);

            return await ExecuteTextAsync(cmd.ToString());
        }

        public void Handle(string message)
        {
            if (message == Notifications.CornerCommand)
            {
                var r = _screenRuler.StickTo(Left, Top);
                Left = r.X;
                Top = r.Y;
                SaveSettings();
            }
            if (message == Notifications.CenterCommand)
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
            AppSettings = _settingsService.Get();
            RefreshAliases();
        }

        public void RefreshAliases(string criterion)
        {
            var l = _aliasService.GetAliasNames(AppSettings.SessionId, criterion);
            AliasNameList = new ObservableCollection<AliasText>(l);
        }

        public void SaveSettings()
        {
            AppSettings.WindowSection.Position.Left = Left;
            AppSettings.WindowSection.Position.Top = Top;

            _settingsService.SavePosition(AppSettings);
            AppSettings = _settingsService.Get();
        }

        private void RefreshAliases()
        {
            var l = _aliasService.GetAliasNames(AppSettings.SessionId);
            AliasNameList = new ObservableCollection<AliasText>(l);
        }

        #endregion Methods
    }
}