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

        [JsonProperty("keyword")]
        public string Keyword { get; set; }


        #endregion Properties
    }
}