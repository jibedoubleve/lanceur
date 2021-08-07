using System;

namespace Probel.Lanceur.SQLiteDb.Entities
{
    internal class UsageEntity : Entity
    {
        #region Properties

        public long IdAlias { get; set; }
        public DateTime Timestamp { get; set; }

        #endregion Properties
    }
}