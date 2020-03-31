namespace Probel.Lanceur.Core.Entities
{
    public class AliasText
    {
        #region Properties

        public long ExecutionCount { get; set; }
        public string FileName { get; set; }
        public string Kind { get; set; }
        public string Name { get; set; }

        #endregion Properties

        #region Methods

        public static AliasText ReservedKeyword(string name)
        {
            return new AliasText
            {
                Name = name,
                ExecutionCount = 0,
                FileName = "Internal Command",
                Kind = "Settings"
            };
        }

        #endregion Methods
    }
}