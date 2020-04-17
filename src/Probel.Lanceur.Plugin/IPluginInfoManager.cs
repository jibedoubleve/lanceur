using System.Collections.Generic;

namespace Probel.Lanceur.Plugin
{
    public interface IPluginInfoManager
    {
        #region Methods

        IEnumerable<IPluginMetadata> GetPluginsInfo();

        #endregion Methods
    }
}