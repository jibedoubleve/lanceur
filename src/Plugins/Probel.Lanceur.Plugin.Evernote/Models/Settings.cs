using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Probel.Lanceur.Plugin.Evernote.Models
{
    internal class Settings
    {
        #region Fields

        private const string DefaultServer = "www.evernote.com";

        #endregion Fields

        #region Properties

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("server")]
        public string Server { get; set; }

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

        internal static void Save(Settings src)
        {
            src.Normalise();

            var json = JsonConvert.SerializeObject(src);
            var p = GetPath();
            File.WriteAllText(p, json);
        }

        private static string GetPath()
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(dir, "api.json");
            return path;
        }

        private void Normalise()
        {
            if (string.IsNullOrEmpty(Server))
            {
                Server = DefaultServer;
            }
        }

        #endregion Methods
    }
}