﻿namespace Probel.Lanceur.Plugin
{
    public interface IPluginMetadata
    {
        #region Properties

        string Description { get; }
        string Dll { get; }
        string Keyword { get; }
        string Name { get; }

        #endregion Properties
    }
}