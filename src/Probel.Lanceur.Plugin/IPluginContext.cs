namespace Probel.Lanceur.Plugin
{
    public interface IPluginContext
    {
        #region Properties

        ILogService LogService { get; }
        IPluginViewAdapter PluginViewAdapter { get; }
        IUserNotifyer UserNotifyer { get; }

        #endregion Properties
    }
}