using MahApps.Metro.Controls.Dialogs;

namespace Probel.Lanceur.Services
{
    public interface IUserNotifyer
    {
        #region Methods

        void NotifyInfo(string message, string title = null);
        MessageDialogResult Ask(string message, string title = null);

        #endregion Methods
    }
}