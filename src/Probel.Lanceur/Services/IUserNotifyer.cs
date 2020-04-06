using MahApps.Metro.Controls.Dialogs;

namespace Probel.Lanceur.Services
{
    public interface IUserNotifyer
    {
        #region Methods

        MessageDialogResult Ask(string message, string title = null);

        void NotifyError(string message, string title = null);

        void NotifyInfo(string message, string title = null);

        void NotifyWarning(string message, string title = null);

        #endregion Methods
    }
}