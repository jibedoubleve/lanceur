using Caliburn.Micro;
using Probel.Lanceur.Core.Helpers;
using Probel.Lanceur.Core.Entities.Settings;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace Probel.Lanceur.ViewModels
{
    public class MainViewModel : Screen, IHandle<string>
    {
        #region Fields

        private readonly IScreenRuler _screenRuler;
        private readonly ISettingsService _settingsService;
        private readonly IShortcutService _shortcutService;
        private AppSettings _appSettings;
        private string _colour;
        private double _left;
        private double _opacity;
        private ObservableCollection<string> _shortcutNameList;
        private double _top;
        private string shortcutName;

        #endregion Fields

        #region Constructors

        public MainViewModel(IShortcutService shortcutService, ISettingsService settings, IEventAggregator ea, IScreenRuler screenRuler, ILogService logService)
        {
            LogService = logService;
            ea.Subscribe(this);
            _screenRuler = screenRuler;
            _settingsService = settings;
            _shortcutService = shortcutService;
        }

        #endregion Constructors

        #region Properties

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

        public double Left
        {
            get => _left;
            set => Set(ref _left, value, nameof(Left));
        }

        public ILogService LogService { get; }

        public double Opacity
        {
            get => _opacity;
            set => Set(ref _opacity, value, nameof(Opacity));
        }

        public string ShortcutName
        {
            get => shortcutName;
            set => Set(ref shortcutName, value, nameof(ShortcutName));
        }

        public ObservableCollection<string> ShortcutNameList
        {
            get => _shortcutNameList;
            set => Set(ref _shortcutNameList, value, nameof(ShortcutNameList));
        }

        public double Top
        {
            get => _top;
            set => Set(ref _top, value, nameof(Top));
        }

        #endregion Properties

        #region Methods

        public void ExecuteText(string cmdLine) => _shortcutService.Execute(cmdLine);

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

        public void LoadSettings()
        {
            var s = _settingsService.Get().WindowSection;
            var pos = s.Position;
            Top = pos.Top;
            Left = pos.Left;
            Colour = s.Colour;
            Opacity = s.Opacity;
        }

        public void LoadShortcuts()
        {
            var l = _shortcutService.GetShortcutsNames(AppSettings.SessionId);
            ShortcutNameList = new ObservableCollection<string>(l);
        }

        public void OnShow()
        {
            AppSettings = _settingsService.Get();
            var n = _shortcutService.GetShortcutsNames(AppSettings.SessionId);
            ShortcutNameList = new ObservableCollection<string>(n);
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