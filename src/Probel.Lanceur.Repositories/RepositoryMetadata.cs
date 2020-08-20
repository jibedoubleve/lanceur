using Newtonsoft.Json;

namespace Probel.Lanceur.Repositories
{
    public class RepositoryMetadata
    {
        #region Properties

        [JsonProperty("dll")]
        public string Dll { get; set; }

        [JsonProperty("explanation")]
        public string Explanation { get; set; }

        [JsonProperty("keyword")]
        public string Keyword { get; set; }

        [JsonProperty("min-ver")]
        public string MinimumVersion { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        #endregion Properties
    }
}