using Probel.Lanceur.SharedKernel.Logs;
using Probel.Lanceur.SharedKernel.UserCom;

namespace Probel.Lanceur.Plugin
{
    public interface IPluginContext
    {
        #region Properties

        ILogService LogService { get; }
        IPluginViewFinder PluginViewAdapter { get; }
        IUserNotifyer UserNotifyer { get; }

        #endregion Properties
    }
}