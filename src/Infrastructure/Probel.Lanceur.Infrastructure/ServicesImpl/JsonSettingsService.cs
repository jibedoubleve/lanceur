using Newtonsoft.Json;
using Probel.Lanceur.Core.Entities.Settings;
using Probel.Lanceur.Core.Services;
using System;
using System.Configuration;
using System.IO;

namespace Probel.Lanceur.Infrastructure.ServicesImpl
{
    public class JsonSettingsService : ISettingsService
    {
        #region Fields

        private readonly string _file;

        #endregion Fields

        #region Constructors

        public JsonSettingsService()
        {
            var path = ConfigurationManager.AppSettings["settings"];
            _file = Environment.ExpandEnvironmentVariables(path);
        }

        #endregion Constructors

        #region Methods

        public AppSettings Get()
        {
            var file = new AppSettings();

            if (File.Exists(_file))
            {
                var json = File.ReadAllText(_file);
                file = JsonConvert.DeserializeObject<AppSettings>(json);
            }
            return file;
        }

        public void Save(AppSettings settings)
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            if (File.Exists(_file)) { File.Delete(_file); }
            File.WriteAllText(_file, json);
        }

        public void SavePosition(AppSettings src)
        {
            var dst = Get();
            dst.WindowSection.Position.Left = src.WindowSection.Position.Left;
            dst.WindowSection.Position.Top = src.WindowSection.Position.Top;

            Save(dst);
        }

        #endregion Methods
    }
}