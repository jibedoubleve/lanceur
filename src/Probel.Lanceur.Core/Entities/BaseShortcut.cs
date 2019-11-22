using Probel.Lanceur.Core.Constants;

namespace Probel.Lanceur.Core.Entities
{
    public abstract class BaseShortcut
    {
        #region Properties

        public string Arguments { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public long Id { get; set; }

        public bool IsExecutable { get; set; } = true;

        public string Notes { get; set; } = string.Empty;

        public RunAs RunAs { get; set; }

        public StartMode StartMode { get; set; }

        #endregion Properties
    }
}