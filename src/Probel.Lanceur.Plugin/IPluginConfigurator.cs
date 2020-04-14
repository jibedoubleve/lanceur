using System.Collections.Generic;

namespace Probel.Lanceur.Plugin
{
    public interface IPluginConfigurator
    {
        #region Methods

        IEnumerable<PluginConfig> GetAllConfigurations();

        void Save(PluginConfig config);

        #endregion Methods
    }

    public class PluginConfig
    {
        #region Properties

        public string Dll { get; set; }

        public string Explanation { get; set; }

        public string FilePath { get; set; }

        public string Keyword { get; set; }

        public string Name { get; set; }

        #endregion Properties
    }
}