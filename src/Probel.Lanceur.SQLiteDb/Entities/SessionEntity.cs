namespace Probel.Lanceur.SQLiteDb.Entities
{
    internal class SessionEntity : Entity
    {
        #region Properties

        public long Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }

        #endregion Properties
    }
}