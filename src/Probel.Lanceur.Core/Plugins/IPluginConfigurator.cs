using Newtonsoft.Json;
using System.Collections.Generic;

namespace Probel.Lanceur.Core.Plugins
{
    public interface IPluginConfigurator
    {
        #region Methods

        IEnumerable<PluginConfig> GetAllConfigurations();

        void Save(PluginConfig config);

        #endregion Methods
    }

    public class PluginConfig
    {
        #region Properties

        [JsonProperty("dll")]
        public string Dll { get; set; }

        [JsonProperty("explanation")]
        public string Explanation { get; set; }

        [JsonIgnore]
        public string FilePath { get; set; }

        [JsonProperty("keyword")]
        public string Keyword { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        #endregion Properties
    }
}