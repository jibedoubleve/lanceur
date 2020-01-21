using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;

namespace Probel.Lanceur.Services
{
    public class UserNotifyer : IUserNotifyer
    {
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

        //TODO: need improvment. UI should be in the Mahapps fashion
        public void NotifyInfo(string message, string title = null)
        {
            MessageBox.Show(message, title ?? "INFORMATION", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion Methods
    }
}