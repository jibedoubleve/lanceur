using Newtonsoft.Json;
using System;
using System.IO;

namespace Probel.Lanceur.Plugin.Spotify.Spotify
{
    public static class JsonConfig
    {
        #region Properties

        private static string Path => Environment.ExpandEnvironmentVariables(@"%appdata%\probel\Lanceur\spotify.json");

        #endregion Properties

        #region Methods

        public static SpotifyConfig Load()
        {
            if (File.Exists(Path) == false) { using (File.Create(Path)) { } }

            var json = File.ReadAllText(Path);
            return JsonConvert.DeserializeObject<SpotifyConfig>(json) ?? new SpotifyConfig();
        }

        public static void Save(SpotifyConfig stg)
        {
            var json = JsonConvert.SerializeObject(stg);
            File.WriteAllText(Path, json);
        }

        #endregion Methods
    }
}