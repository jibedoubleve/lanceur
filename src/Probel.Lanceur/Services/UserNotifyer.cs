using MahApps.Metro.Controls.Dialogs;
using Notifications.Wpf;
using System;
using System.Windows;

namespace Probel.Lanceur.Services
{
    public class UserNotifyer : IUserNotifyer
    {
        #region Fields

        private readonly INotificationManager _notifyer;

        #endregion Fields

        #region Constructors

        public UserNotifyer(INotificationManager notifyer)
        {
            _notifyer = notifyer;
        }

        #endregion Constructors

        #region Methods

        //TODO: need improvment. UI should be in the Mahapps fashion
        public MessageDialogResult Ask(string message, string title = null)
        {
            var result = MessageBox.Show(message, title ?? "QUESTION", MessageBoxButton.OK, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.None: return MessageDialogResult.Canceled;
                case MessageBoxResult.OK:
                case MessageBoxResult.Yes: return MessageDialogResult.Affirmative;
                case MessageBoxResult.Cancel:
                case MessageBoxResult.No: return MessageDialogResult.Negative;
                default: throw new NotSupportedException($"The answer '{result}' is not supported.");
            }
        }

        public void NotifyError(string message, string title = null) => Notify(message, title, NotificationType.Error);

        public void NotifyInfo(string message, string title = null) => Notify(message, title, NotificationType.Information);

        public void NotifyWarning(string message, string title = null) => Notify(message, title, NotificationType.Warning);

        private void Notify(string message, string title, NotificationType type)
        {
            var m = new NotificationContent
            {
                Title = title ?? type.ToString().ToUpper(),
                Message = message,
                Type = type
            };
            _notifyer.Show(m);
        }

        #endregion Methods
    }
}