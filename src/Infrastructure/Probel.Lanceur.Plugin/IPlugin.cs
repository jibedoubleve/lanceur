using Probel.Lanceur.Core.Entities;

namespace Probel.Lanceur.Plugin
{
    public interface IPlugin
    {
        #region Methods

        void Execute(PluginCmdline parameters);

        void Initialise(IPluginContext context);

        #endregion Methods
    }
}