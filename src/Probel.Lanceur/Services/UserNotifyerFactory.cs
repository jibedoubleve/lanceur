using Probel.Lanceur.Core.Entities.Settings;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.SharedKernel.UserCom;
using Unity;

namespace Probel.Lanceur.Services
{
    public class UserNotifyerFactory : IUserNotifyerFactory
    {
        #region Fields

        private readonly IUnityContainer _container;
        private readonly AppSettings _settings;

        #endregion Fields

        #region Constructors

        public UserNotifyerFactory(IUnityContainer container, ISettingsService settingsService)
        {
            _settings = settingsService.Get();
            _container = container;
        }

        #endregion Constructors

        #region Methods

        public IUserNotifyer Get()
        {
            return _container.Resolve<IUserNotifyer>(_settings.WindowSection.NotificationType);
        }

        #endregion Methods
    }
}