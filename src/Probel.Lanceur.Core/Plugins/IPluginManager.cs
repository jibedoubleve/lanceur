using Probel.Lanceur.Core.Entities;
using System.Collections.Generic;

namespace Probel.Lanceur.Core.Plugins
{
    public interface IPluginManager
    {
        #region Methods

        IPlugin Build(string name);

        bool Exists(string name);

        IEnumerable<AliasText> GetKeywords();

        #endregion Methods
    }
}