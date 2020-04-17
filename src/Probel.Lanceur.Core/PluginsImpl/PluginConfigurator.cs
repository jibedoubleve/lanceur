using Newtonsoft.Json;
using Probel.Lanceur.Plugin;
using System;
using System.Collections.Generic;
using System.IO;

namespace Probel.Lanceur.Core.PluginsImpl
{
    public class PluginConfigurator : IPluginConfigurator
    {
        #region Fields

        private string _repository = Environment.ExpandEnvironmentVariables(PluginManager.PluginRepository);

        #endregion Fields

        #region Methods

        public IEnumerable<PluginConfig> GetAllConfigurations()
        {
            var cfgs = new List<PluginConfig>();
            foreach (var file in Directory.EnumerateFiles(_repository, "plugin.config.json", SearchOption.AllDirectories))
            {
                var json = File.ReadAllText(file);

                var tmp = JsonConvert.DeserializeObject<PluginConfig>(json);
                tmp.FilePath = file;
                cfgs.Add(tmp);
            }

            return cfgs;
        }

        public void Save(PluginConfig config)
        {
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(config.FilePath, json);
        }

        #endregion Methods
    }
}