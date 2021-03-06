﻿using Caliburn.Micro;
using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.SharedKernel.UserCom;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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

        public EditDoubloonsViewModel(IUserNotifyerFactory factory,
            IDataSourceService dataService,
            ISettingsService settingService)
        {
            _settingService = settingService;
            _notifyer = factory.Get();
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

        public async Task DeleteCurrentAsync(long id)
        {
            var response = await _notifyer.AskAsync("Do you want to delete this doubloon?");
            if (response == NotificationResult.Affirmative)
            {
                _dataService.Delete((Alias)id);
                RefreshData();
            }
        }

        public void RefreshData()
        {
            _notifyer.NotifyWait();
            var s = _settingService.Get();
            var doubloons = _dataService.GetDoubloons(s.SessionId);
            Doubloons = new ObservableCollection<Doubloon>(doubloons);
            _notifyer.NotifyEndWait();
        }

        #endregion Methods
    }
}