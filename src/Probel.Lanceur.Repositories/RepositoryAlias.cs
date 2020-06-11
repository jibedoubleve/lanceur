namespace Probel.Lanceur.Repositories
{    //TODO: can be a AliasText as it has the same properties
    public class RepositoryAlias
    {
        #region Properties

        public long ExecutionCount { get; set; }
        public string FileName { get; set; }
        public bool IsExecutable => true;
        public string Kind { get; set; }
        public string Name { get; set; }

        #endregion Properties
    }
}