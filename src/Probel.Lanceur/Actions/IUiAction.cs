using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Services;
using Unity;

namespace Probel.Lanceur.Actions
{
    public interface IUiAction
    {
        #region Methods

        void Execute(string arg);

        IUiAction With(IUnityContainer container, IDataSourceService dataService, ILogService log, IUserNotifyer notifyer);

        #endregion Methods
    }
}