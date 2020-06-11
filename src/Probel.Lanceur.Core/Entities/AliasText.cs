using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Plugin;
using Probel.Lanceur.Repositories;

namespace Probel.Lanceur.Core.Entities
{
    public class AliasText
    {
        #region Properties

        public long ExecutionCount { get; set; }
        public string FileName { get; set; }

        /// <summary>
        /// Indicates whether the alias can be executed as is or
        /// Further information is needed to execute it. For instance
        /// MagicWords are not executable as you need to retrieve
        /// information into the DB to execute it.
        /// </summary>
        public bool IsExecutable { get; set; } = false;

        public virtual string Kind { get; set; }
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
                IsExecutable = src.IsExecutable,
            };
        }

        public static implicit operator AliasText(RepositoryAlias src)
        {
            return new AliasText
            {
                ExecutionCount = src.ExecutionCount,
                FileName = src.FileName,
                Kind = src.Kind,
                Name = src.Name,
                IsExecutable = src.IsExecutable,
            };
        }

        public static AliasText ReservedKeyword(ActionWord word)
        {
            return new AliasText
            {
                Name = word.Name.ToLower(),
                ExecutionCount = 0,
                FileName = $"{word.Description}",
                Kind = "Settings",
            };
        }

        public string AsCommandLine(string parameters = null) => $"{Name} {parameters}".Trim();

        #endregion Methods
    }
}