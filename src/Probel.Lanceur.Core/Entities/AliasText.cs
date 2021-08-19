using Probel.Lanceur.Core.Services;
using System;

namespace Probel.Lanceur.Core.Entities
{
    public class AliasText
    {
        #region Fields

        private string _name;

        #endregion Fields

        #region Properties

        public long ExecutionCount { get; set; }
        public string FileName { get; set; }
        public string Icon { get; set; }
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

        public double SearchScore { get; set; }

        /// <summary>
        /// Represents a unique id of the UWP packaged application. If
        /// <see cref="IsPackaged"/> is set to <c>False</c> then, this
        /// property should be <c>NULL</c>
        /// </summary>
        public string UniqueIdentifier { get; set; }

        #endregion Properties

        #region Methods

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

        public static AliasText FromText(string cmdline)
        {
            if (string.IsNullOrEmpty(cmdline)) { throw new ArgumentException($"The command line should NOT be empty or null", nameof(cmdline)); }

            var index = cmdline.IndexOf(' ');
            var cmd = cmdline.Substring(0, index)?.Trim();
            var @params = cmdline.Substring(index, cmdline.Length - index)?.Trim();

            return new AliasText
            {
                FileName = cmd,
                Name = cmd,
            };
        }

        public string AsCommandLine(string parameters = null) => $"{Name} {parameters}".Trim();

        #endregion Methods
    }
}