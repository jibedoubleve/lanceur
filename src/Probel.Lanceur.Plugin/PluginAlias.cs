﻿namespace Probel.Lanceur.Plugin
{
    //TODO: can be a AliasText as it has the same properties
    public class PluginAlias
    {
        #region Properties

        public long ExecutionCount { get; set; }
        public string FileName { get; set; }
        public bool IsExecutable => false;
        public string Kind { get; set; }
        public string Name { get; set; }

        #endregion Properties
    }
}