using System;

namespace Probel.Lanceur.Sqlite.Entities
{
    public class Usage : Entity
    {
        #region Properties

        public long IdShortcut { get; set; }
        public DateTime Timestamp { get; set; }

        #endregion Properties
    }
}