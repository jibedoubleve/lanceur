using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Services;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Probel.Lanceur.ViewModels
{
    public class EditObsoleteKeywordsViewModel : Screen
    {
        #region Fields

        private readonly IDataSourceService _dataService;
        private readonly IUserNotifyer _notifyer;
        private readonly ISettingsService _settingService;
        private ObservableCollection<Alias> _emptyKeywords;

        #endregion Fields

        #region Constructors

        public EditObsoleteKeywordsViewModel(IDataSourceService dataService,
            ISettingsService settingService,
            IUserNotifyer notifyer)
        {
            _notifyer = notifyer;
            _settingService = settingService;
            _dataService = dataService;
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<Alias> EmptyKeywords
        {
            get => _emptyKeywords;
            set => Set(ref _emptyKeywords, value, nameof(EmptyKeywords));
        }

        #endregion Properties

        #region Methods

        public async Task DeleteCurrent(long id)
        {
            var response = await _notifyer.AskAsync("Do you want to delete this doubloon?");
            if (response == NotificationResult.Affirmative)
            {
                _dataService.Delete(id);
                RefreshData();
            }
        }

        /// <summary>
        /// Checks into the absolute paths if the files still exists.
        /// This is an aproximate search, it could be that some words
        /// are dead and not taken in the filter.
        /// </summary>
        public void RefreshData()
        {

            _notifyer.NotifyWait();
            var macro = new Regex("^@.*@$");
            var abs = new Regex(@"[a-zA-Z]:\\");

            var s = _settingService.Get();
            var result = (from a in _dataService.GetAliases(s.SessionId)
                          where Uri.TryCreate(a.FileName, UriKind.RelativeOrAbsolute, out _) == true
                             && File.Exists(a.FileName) == false
                             && macro.IsMatch(a.FileName) == false
                             && abs.IsMatch(a.FileName)
                          select a);
            EmptyKeywords = new ObservableCollection<Alias>(result);
            _notifyer.NotifyEndWait();
        }

        #endregion Methods
    }
}