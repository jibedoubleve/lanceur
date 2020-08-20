﻿using System.Threading.Tasks;

namespace Probel.Lanceur.Plugin
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

        Task<NotificationResult> AskAsync(string message, string title = null);

        //NotificationResult Ask(string message, string title = null);
        void NotifyEndWait();

        void NotifyError(string message, string title = null);

        void NotifyInfo(string message, string title = null);

        void NotifyWait();

        void NotifyWarning(string message, string title = null);

        void SetDialogSource(object src);

        #endregion Methods
    }

    public interface IUserNotifyerFactory
    {
        IUserNotifyer Get();
    }
}