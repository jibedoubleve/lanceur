using Newtonsoft.Json;
using System;

namespace Probel.Lanceur.Core.Plugins
{
    public class PluginMetadata : IPluginMetadata
    {
        #region Properties

        [JsonProperty("explanation")]
        public string Description { get; set; }

        [JsonProperty("dll")]
        public string Dll { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Should be the guid of the plugin. Information is in the
        /// plugin.config.json
        /// </summary>
        [JsonProperty("plugin-id")]
        public Guid PluginId { get; set; }

        #endregion Properties
    }
}