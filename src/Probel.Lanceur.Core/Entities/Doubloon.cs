namespace Probel.Lanceur.Core.Entities
{
    public class Doubloon
    {
        #region Properties

        public string Arguments { get; set; }
        public string FileName { get; set; }
        public long Id { get; set; }
        public long IdSession { get; set; }
        public string Keywords { get; set; }
        public int RunAs { get; set; }
        public string WorkingDir { get; set; }

        #endregion Properties
    }
}