namespace Probel.Lanceur.Services
{
    public enum NotificationResult
    {
        Negative,
        Affirmative,
        Canceled,
    }

    public interface IUserNotifyer
    {
        #region Methods

        NotificationResult Ask(string message, string title = null);

        void NotifyError(string message, string title = null);

        void NotifyInfo(string message, string title = null);

        void NotifyWarning(string message, string title = null);

        void NotifyWait();

        void NotifyEndWait();

        #endregion Methods
    }
}