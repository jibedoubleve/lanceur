using System.Collections.Generic;

namespace Probel.Lanceur.Core.Plugins
{
    public interface IPluginInfoManager
    {
        #region Methods

        IEnumerable<IPluginMetadata> GetPluginsInfo();

        #endregion Methods
    }
}