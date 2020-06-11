using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Plugin;

namespace Probel.Lanceur.Core.PluginsImpl
{
    internal class EmptyPlugin : PluginBase
    {
        #region Methods

        public override void Execute(Cmdline cmd)
        {
            Logger.Warning("User tried to execute an empty plugin.");
        }

        #endregion Methods
    }
}