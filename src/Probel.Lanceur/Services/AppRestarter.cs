using Probel.Lanceur.Infrastructure;
using Probel.Lanceur.Plugin;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace Probel.Lanceur.Services
{
    public class AppRestarter : IAppRestarter
    {
        #region Fields

        private readonly IUserNotifyer _notifyer;

        #endregion Fields

        #region Constructors

        public AppRestarter(IUserNotifyerFactory factory)
        {
            _notifyer = factory.Get();
        }

        #endregion Constructors

        #region Methods

        public async Task<bool> DoRestartAsync()
        {
            var result = await _notifyer.AskAsync("Do you want to restart?");
            return (result == NotificationResult.Affirmative);
        }

        public void Restart()
        {
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        #endregion Methods
    }
}