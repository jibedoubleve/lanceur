using Probel.Lanceur.Core.Constants;

namespace Probel.Lanceur.SQLiteDb.Entities
{
    internal class AliasEntity : Entity
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