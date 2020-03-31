using System;

namespace Probel.Lanceur.Core.Plugins
{
    public interface IPluginMetadata
    {
        #region Properties

        string Description { get; }
        string Dll { get; }
        string Name { get; }
        Guid PluginId { get; }

        #endregion Properties
    }
}