using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Reflection;

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
        public string Server { get; set; } = "sandbox.evernote.com";

        #endregion Properties

        #region Methods

        internal static Settings Load()
        {
            var p = GetPath();
            var content = File.ReadAllText(p);
            var s = JsonConvert.DeserializeObject<Settings>(content);

            Debug.WriteLine($"Host: {s.Host} - Key: {s.Key} - Server: {s.Server}");

            return s;
        }

        private static string GetPath()
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(dir, "api.json");
            return path;
        }

        #endregion Methods
    }
}