﻿using Caliburn.Micro;
using Probel.Lanceur.Plugin;
using Probel.Lanceur.SharedKernel.UserCom;
using System.Collections.ObjectModel;

namespace Probel.Lanceur.ViewModels
{
    public class EditPluginViewModel : Screen
    {
        #region Fields

        private static bool _isRebootNeeded;
        private readonly IUserNotifyer _notifyer;

        private readonly IPluginConfigurator _pluginConfigurator;

        private ObservableCollection<PluginConfig> _configurations;

        #endregion Fields

        #region Constructors

        public EditPluginViewModel(IPluginConfigurator pluginConfigurator, IUserNotifyerFactory factory)
        {
            _notifyer = factory.Get();
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