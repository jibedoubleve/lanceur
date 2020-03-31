using System;
using System.Collections.Generic;

namespace Probel.Lanceur.Core.Plugins
{
    public interface IPluginLoader
    {
        #region Methods

        IList<IPluginMetadata> LoadPlugins(string pluginRepository, Dictionary<string, Type> pluginTypes);

        #endregion Methods
    }
}