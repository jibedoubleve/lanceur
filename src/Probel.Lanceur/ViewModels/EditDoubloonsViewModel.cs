using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Probel.Lanceur.ViewModels
{
    public class EditDoubloonsViewModel : Screen
    {
        #region Fields

        private readonly IDataSourceService _dataService;

        private readonly ISettingsService _settingService;

        private ObservableCollection<Doubloon> _doubloons;

        #endregion Fields

        #region Constructors

        public EditDoubloonsViewModel(IUserNotifyer notifyer,
            IDataSourceService dataService,
            ISettingsService settingService)
        {
            _settingService = settingService;
            _notifyer = notifyer;
            _dataService = dataService;
        }

        #endregion Constructors

        #region Properties

        public IUserNotifyer _notifyer { get; }

        public ObservableCollection<Doubloon> Doubloons
        {
            get => _doubloons;
            set => Set(ref _doubloons, value, nameof(Doubloons));
        }

        #endregion Properties

        #region Methods

        public void DeleteCurrent(long id)
        {
            var response = _notifyer.Ask("Do you want to delete this doubloon?");
            if (response == MessageDialogResult.Affirmative)
            {
                _dataService.Delete(id);
                RefreshData();
            }
        }

        public void RefreshData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            var s = _settingService.Get();
            var doubloons = _dataService.GetDoubloons(s.SessionId);
            Doubloons = new ObservableCollection<Doubloon>(doubloons);
            Mouse.OverrideCursor = null;
        }

        #endregion Methods
    }
}