using Newtonsoft.Json;
using Probel.Lanceur.Plugin.Evernote.Models;
using System.IO;
using System.Reflection;

namespace Probel.Lanceur.Plugin.Evernote.Services
{
    internal static class SettingsService
    {
        #region Properties

        private static string FilePath
        {
            get
            {
                var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var file = "evernote.config.json";
                var path = Path.Combine(dir, file);
                return path;
            }
        }

        #endregion Properties

        #region Methods

        public static Settings Get()
        {
            var content = File.ReadAllText(FilePath);
            var result = JsonConvert.DeserializeObject<Settings>(content);
            return result;
        }

        public static bool IsEmpty(this Settings s) => string.IsNullOrWhiteSpace(s.Host) || string.IsNullOrWhiteSpace(s.Key);

        public static void Save(this Settings settings)
        {
            var content = JsonConvert.SerializeObject(settings);
            File.WriteAllText(FilePath, content);
        }

        #endregion Methods
    }
}