using Probel.Lanceur.Core.Constants;

namespace Probel.Lanceur.Core.Entities
{
    public abstract class BaseAlias
    {
        #region Properties

        public string Arguments { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public string Icon { get; set; }

        public long Id { get; set; }

        /// <summary>
        /// Indicates whether this alias can be executed as an alias.
        /// If returns <c>True</c>, this is an alias with execution
        /// information into the database. Otherwise, it is either
        /// a reserved keyword or nothing (neither a keyword nor
        /// an alias)
        /// </summary>
        public bool IsExecutable { get; set; } = true;

        public string Notes { get; set; } = string.Empty;

        public RunAs RunAs { get; set; } = RunAs.CurrentUser;

        public StartMode StartMode { get; set; } = StartMode.Default;

        public string WorkingDirectory { get; set; }

        #endregion Properties
    }
}