using Probel.Lanceur.Core.Entities;

namespace Probel.Lanceur.Plugin
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


        public static implicit operator Query(PluginAlias src)
        {
            return new Query
            {
                ExecutionCount = src.ExecutionCount,
                FileName = src.FileName,
                Kind = src.Kind,
                Name = src.Name,
                IsExecutable = src.IsExecutable,
            };
        }

        #endregion Properties
    }
}