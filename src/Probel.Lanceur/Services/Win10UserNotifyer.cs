using MahApps.Metro.Controls.Dialogs;
using Notifications.Wpf;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Helpers;
using Probel.Lanceur.Infrastructure;
using System;
using Windows.UI.Notifications;

namespace Probel.Lanceur.Services
{
    public class Win10UserNotifyer : BaseNotifyer
    {
        #region Constructors

        public Win10UserNotifyer(IDialogCoordinator dialog, ILogService log, ISettingsService settingsService) : base(dialog, log, settingsService)
        {
        }

        #endregion Constructors

        #region Methods

        protected override void Notify(string message, string title, NotificationType type)
        {
            title = title ?? type.ToString();

            var m = ToastMessage.ImageAndText03("./Assets/Notification.ico");
            m.AppendText(title, message);

            // Create the toast and attach event listeners
            var toast = new ToastNotification(m.XmlDocument);

            // Show the toast. Be sure to specify the AppUserModelId on your application's shortcut!
            ToastNotificationManager
                .CreateToastNotifier(ToastMessage.ApplicationId)
                .Show(toast);
        }

        #endregion Methods
    }

    public class UserNotifyer : BaseNotifyer
    {
        #region Fields

        private readonly INotificationManager _notifyer;

        #endregion Fields

        #region Constructors

        public UserNotifyer(INotificationManager notifyer, IDialogCoordinator dialog, ILogService log, ISettingsService settingsService) : base(dialog, log, settingsService)
        {
            _notifyer = notifyer;
        }

        #endregion Constructors

        #region Methods

        protected override void Notify(string message, string title, NotificationType type)
        {
            var m = new NotificationContent
            {
                Title = title ?? type.ToString().ToUpper(),
                Message = message,
                Type = type,
            };

            var expTime = TimeSpan.FromSeconds(SettingsService.Get().WindowSection.ExpirationTimeMessage);
            _notifyer.Show(m, "", expTime);
        }

        #endregion Methods
    }
}