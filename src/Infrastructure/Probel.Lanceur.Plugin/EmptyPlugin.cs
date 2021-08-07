using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Plugin;

namespace Probel.Lanceur.Plugin
{
    public class EmptyPlugin : PluginBase
    {
        #region Methods

        public override void Execute(PluginCmdline cmd)
        {
            Logger.Warning("User tried to execute an empty plugin.");
        }

        #endregion Methods
    }
}