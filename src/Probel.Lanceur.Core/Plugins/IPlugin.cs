using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;

namespace Probel.Lanceur.Core.Plugins
{
    public interface IPlugin
    {
        #region Methods

        void Execute(Cmdline parameters);

        void Initialise(ILogService logger, IApplicationManager mainViewModel);

        #endregion Methods
    }
}