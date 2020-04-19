using System.Collections.Generic;

namespace Probel.Lanceur.Plugin
{
    public interface IPluginManager
    {
        #region Methods

        IPlugin Build(string name);

        void Execute(Cmdline cmd);

        bool Exists(string name);

        IEnumerable<PluginAlias> GetKeywords();

        #endregion Methods
    }
}