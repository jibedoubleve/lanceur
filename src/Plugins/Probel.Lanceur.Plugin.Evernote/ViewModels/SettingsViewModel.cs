using Caliburn.Micro;
using Probel.Lanceur.Plugin.Evernote.Models;
using Probel.Lanceur.Plugin.Evernote.Services;

namespace Probel.Lanceur.Plugin.Evernote.ViewModels
{
    internal class SettingsViewModel : Screen
    {
        #region Fields

        private string _host;
        private string _key;

        #endregion Fields

        #region Properties

        public string Host
        {
            get => _host;
            set => Set(ref _host, value, nameof(Host));
        }

        public string Key
        {
            get => _key;
            set => Set(ref _key, value, nameof(Key));
        }

        #endregion Properties

        #region Methods

        public void RefreshDate()
        {
            var s = SettingsService.Get();
            Host = s.Host;
            Key = s.Key;
        }

        public void SaveSettings() => new Settings { Host = Host, Key = Key }.Save();

        #endregion Methods
    }
}