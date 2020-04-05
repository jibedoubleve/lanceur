using Caliburn.Micro;
using Probel.Lanceur.Core.Plugins;
using Probel.Lanceur.Services;
using System.Collections.ObjectModel;

namespace Probel.Lanceur.ViewModels
{
    public class EditPluginViewModel : Screen
    {
        #region Fields

        private readonly IUserNotifyer _notifyer;

        private readonly IPluginConfigurator _pluginConfigurator;

        private ObservableCollection<PluginConfig> _configurations;

        private static bool _isRebootNeeded;

        #endregion Fields

        #region Constructors

        public EditPluginViewModel(IPluginConfigurator pluginConfigurator, IUserNotifyer notifyer)
        {
            _notifyer = notifyer;
            _pluginConfigurator = pluginConfigurator;
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<PluginConfig> Configurations
        {
            get => _configurations;
            set => Set(ref _configurations, value, nameof(Configurations));
        }

        public bool IsRebootNeeded
        {
            get => _isRebootNeeded;
            set => Set(ref _isRebootNeeded, value, nameof(IsRebootNeeded));
        }

        #endregion Properties

        #region Methods

        public void RefreshData()
        {
            var c = _pluginConfigurator.GetAllConfigurations();
            Configurations = new ObservableCollection<PluginConfig>(c);
        }

        public void Reset() => RefreshData();

        public void Save()
        {
            foreach (var cfg in Configurations) { _pluginConfigurator.Save(cfg); }
            _notifyer.NotifyInfo("Saved changes done the plugins configuration.");
            IsRebootNeeded = true;
        }

        #endregion Methods
    }
}