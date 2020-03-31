using Newtonsoft.Json;

namespace Probel.Lanceur.Plugin.Spotify.Spotify
{
    public class SpotifyConfig
    {
        #region Properties

        [JsonProperty("access-token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh-token")]
        public string RefreshToken { get; set; }

        #endregion Properties
    }
}