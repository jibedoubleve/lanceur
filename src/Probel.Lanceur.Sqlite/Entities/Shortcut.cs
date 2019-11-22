using Probel.Lanceur.Core.Models;

namespace Probel.Lanceur.Sqlite.Entities
{
    public class Shortcut : Entity
    {
        #region Properties

        public string Arguments { get; set; }
        public string FileName { get; set; }
        public string Notes { get; set; }
        public RunAs RunAs { get; set; }
        public StartMode StartMode { get; set; }

        #endregion Properties
    }
}