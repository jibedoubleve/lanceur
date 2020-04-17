using Newtonsoft.Json;

namespace Probel.Lanceur.Plugin.Evernote.Models
{
    internal class Settings
    {
        #region Properties

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("server")]
        public string Server { get; } = "sandbox.evernote.com";

        #endregion Properties
    }
}