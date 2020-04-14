using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Plugin;

namespace Probel.Lanceur.Core.Entities
{
    public class AliasText
    {
        #region Properties

        public long ExecutionCount { get; set; }
        public string FileName { get; set; }
        public string Kind { get; set; }
        public string Name { get; set; }

        #endregion Properties

        #region Methods

        public static implicit operator AliasText(PluginAlias src)
        {
            return new AliasText
            {
                ExecutionCount = src.ExecutionCount,
                FileName = src.FileName,
                Kind = src.Kind,
                Name = src.Name,
            };
        }

        public static AliasText ReservedKeyword(ActionWord word)
        {
            return new AliasText
            {
                Name = word.Name.ToLower(),
                ExecutionCount = 0,
                FileName = $"{word.Description}",
                Kind = "Settings"
            };
        }

        #endregion Methods
    }
}