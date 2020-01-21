using Caliburn.Micro;
using Probel.Lanceur.Core.Entities.Settings;
using Probel.Lanceur.Core.Helpers;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Services;
using System;
using System.Collections.ObjectModel;

namespace Probel.Lanceur.ViewModels
{
    public class MainViewModel : Screen, IHandle<string>
    {
        #region Fields

        private readonly IAliasService _aliasService;
        private readonly string _colour;
        private readonly IScreenRuler _screenRuler;
        private readonly ISettingsService _settingsService;
        private string _aliasName;
        private ObservableCollection<string> _aliasNameList;
        private AppSettings _appSettings;
        private bool _isOnError;
        private double _left;
        private double _opacity;
        private double _top;

        #endregion Fields

        #region Constructors

        public MainViewModel(IAliasService aliasService, ISettingsService settings, IEventAggregator ea, IScreenRuler screenRuler, ILogService logService)
        {
            LogService = logService;
            ea.Subscribe(this);
            _screenRuler = screenRuler;
            _settingsService = settings;
            _aliasService = aliasService;
        }

        #endregion Constructors

        #region Properties

        public string AliasName
        {
            get => _aliasName;
            set => Set(ref _aliasName, value, nameof(AliasName));
        }

        public ObservableCollection<string> AliasNameList
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
            get => "Crimson";
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
        public bool ExecuteText(string cmdLine)
        {
            try { return _aliasService.Execute(cmdLine); }
            catch (Exception ex)
            {
                /* I swallow the error as this crash should'nt crash the application
                 * I log and continue without any other warning.
                 */
                LogService.Warning($"An error occured while trying to execute the alias '{cmdLine}'", ex);
                return false;
            }
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
        }

        public void LoadAliases()
        {
            var l = _aliasService.GetAliasNames(AppSettings.SessionId);
            AliasNameList = new ObservableCollection<string>(l);
        }

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
            var n = _aliasService.GetAliasNames(AppSettings.SessionId);
            AliasNameList = new ObservableCollection<string>(n);
        }

        public void SaveSettings()
        {
            AppSettings.WindowSection.Position.Left = Left;
            AppSettings.WindowSection.Position.Top = Top;

            _settingsService.Save(AppSettings);
            AppSettings = _settingsService.Get();
        }

        #endregion Methods
    }
}