using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace Probel.Lanceur.Plugin.Evernote
{
    public class Plugin : PluginBase
    {
        #region Fields

        private Settings _settings = null;        
            

        #endregion Fields

        #region Properties

        private Settings Settings
        {
            get
            {
                if (_settings == null)
                {
                    var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    var file = "plugin.config.json";
                    var path = Path.Combine(dir, file);

                    var content = File.ReadAllText(path);
                    _settings = JsonConvert.DeserializeObject<Settings>(content);
                }
                return _settings;
            }
        }

        #endregion Properties

        #region Methods

        public override void Execute(Cmdline cmd)
        {
            var h = Settings.Host;
        }

        #endregion Methods
    }
}