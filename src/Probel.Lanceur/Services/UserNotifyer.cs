using MahApps.Metro.Controls.Dialogs;
using Notifications.Wpf;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.SharedKernel.Logs;
using Probel.Lanceur.SharedKernel.UserCom;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Probel.Lanceur.Services
{
    public abstract class BaseNotifyer : IUserNotifyer
    {
        #region Fields

        private static object _dialogSource;
        private readonly IDialogCoordinator _dialog;

        #endregion Fields

        #region Constructors

        protected BaseNotifyer(IDialogCoordinator dialog, ILogService log, ISettingsService settingsService)
        {
            SettingsService = settingsService;
            Log = log;
            _dialog = dialog;
        }

        #endregion Constructors

        #region Properties

        protected ILogService Log { get; private set; }
        protected ISettingsService SettingsService { get; private set; }

        #endregion Properties

        #region Methods

        private NotificationResult Ask(string message, string title = null)
        {
            var result = MessageBox.Show(message, title ?? "QUESTION", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.None: return NotificationResult.Canceled;
                case MessageBoxResult.OK:
                case MessageBoxResult.Yes: return NotificationResult.Affirmative;
                case MessageBoxResult.Cancel:
                case MessageBoxResult.No: return NotificationResult.Negative;
                default: throw new NotSupportedException($"The answer '{result}' is not supported.");
            }
        }

        protected abstract void Notify(string message, string title, NotificationType type);

        public async Task<NotificationResult> AskAsync(string message, string title = null)
        {
            if (_dialogSource == null)
            {
                var result = Ask(message, title);
                return await Task.FromResult(result);
            }
            else
            {
                var opt = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                };

                var result = MessageDialogResult.Negative;
                try
                {
                    result = await _dialog.ShowMessageAsync(_dialogSource, title ?? "QUESTION", message, MessageDialogStyle.AffirmativeAndNegative, opt);
                }
                catch (Exception ex) { Log.Warning(ex); }

                switch (result)
                {
                    case MessageDialogResult.Canceled: return NotificationResult.Canceled;
                    case MessageDialogResult.Negative: return NotificationResult.Negative;
                    case MessageDialogResult.Affirmative: return NotificationResult.Affirmative;
                    default: throw new NotSupportedException($"The result '{result}' is not supported as an answer.");
                };
            }
        }

        public void NotifyEndWait() => Mouse.OverrideCursor = null;

        public void NotifyError(string message, string title = null) => Notify(message, title, NotificationType.Error);

        public void NotifyInfo(string message, string title = null) => Notify(message, title, NotificationType.Information);

        public void NotifyWait() => Mouse.OverrideCursor = Cursors.Wait;

        public void NotifyWarning(string message, string title = null) => Notify(message, title, NotificationType.Warning);

        public void SetDialogSource(object src) => _dialogSource = src;

        #endregion Methods
    }
}