using Probel.Lanceur.Core.Plugins;
using System;
using System.Collections.Generic;

namespace Probel.Lanceur.Core.PluginsImpl
{
    internal class PluginConfigurator : IPluginConfigurator
    {
        #region Fields

        private string _pluginRepository;

        #endregion Fields

        #region Constructors

        public PluginConfigurator(string pluginRepository)
        {
            _pluginRepository = Environment.ExpandEnvironmentVariables(pluginRepository);
        }

        #endregion Constructors

        #region Methods

        public IEnumerable<PluginConfig> GetAllConfigurations()
        {
            var configs = new List<PluginConfig>();
                return configs;
        }

        public void Save(PluginConfig config) => throw new System.NotImplementedException();

        #endregion Methods
    }
}