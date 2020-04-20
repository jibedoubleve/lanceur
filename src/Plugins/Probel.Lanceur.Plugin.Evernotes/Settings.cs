using Newtonsoft.Json;

namespace Probel.Lanceur.Plugin.Evernote
{
    public class Settings
    {
        #region Properties

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        #endregion Properties
    }
}