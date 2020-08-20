using Newtonsoft.Json;
using Probel.Lanceur.Plugin;

namespace Probel.Lanceur.Core.PluginsImpl
{
    public class PluginMetadata : IPluginMetadata
    {
        #region Properties

        [JsonProperty("explanation")]
        public string Description { get; set; }

        [JsonProperty("dll")]
        public string Dll { get; set; }

        [JsonProperty("keyword")]
        public string Keyword { get; set; }

        [JsonProperty("min-ver")]
        public string MinimumVersion { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        #endregion Properties
    }
}