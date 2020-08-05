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
        public string Icon { get; protected set; }
        public long Id { get; set; }

        /// <summary>
        /// Indicates whether the alias can be executed as is or
        /// Further information is needed to execute it. For instance
        /// MagicWords are not executable as you need to retrieve
        /// information into the DB to execute it.
        /// </summary>
        public bool IsExecutable { get; set; } = true;

        /// <summary>
        /// Indicates whether the alias is a packaged application (UWP)
        /// </summary>
        public bool IsPackaged { get; set; }

        public virtual string Kind { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NameLowercase = value.ToLower();
            }
        }

        public string NameLowercase { get; private set; }

        public double SearchScore { get; protected set; }

        /// <summary>
        /// Represents a unique id of the UWP packaged application. If
        /// <see cref="IsPackaged"/> is set to <c>False</c> then, this
        /// property should be <c>NULL</c>
        /// </summary>
        public string UniqueIdentifier { get; set; }

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
            var r = new AliasText
            {
                ExecutionCount = src.ExecutionCount,
                FileName = src.FileName,
                Kind = src.Kind,
                Name = src.Name,
                IsExecutable = src.IsExecutable,
                IsPackaged = src.IsPackaged,
                UniqueIdentifier = src.UniqueIdentifier,
                Icon = src.Icon,
                SearchScore = src.SearchScore,
            };
            return r;
        }

        public static AliasText ReservedKeyword(ActionWord word)
        {
            return new AliasText
            {
                Name = word.Name.ToLower(),
                ExecutionCount = 0,
                FileName = $"{word.Description}",
                IsExecutable = false,
                Kind = "Settings",
            };
        }

        public string AsCommandLine(string parameters = null) => $"{Name} {parameters}".Trim();

        #endregion Methods
    }
}