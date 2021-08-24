using Probel.Lanceur.Core.Entities;

namespace Probel.Lanceur.Repositories
{    //TODO: can be a AliasText as it has the same properties
    public class RepositoryAlias
    {
        #region Properties

        public long ExecutionCount { get; set; }
        public string FileName { get; set; }
        public string Icon { get; set; }
        public bool IsExecutable => true;
        public bool IsPackaged { get; set; } = false;
        public string Kind { get; set; }
        public string Name { get; set; }
        public double SearchScore { get; set; }
        public string UniqueIdentifier { get; set; }

        #endregion Properties

        #region Methods

        public static implicit operator Query(RepositoryAlias src)
        {
            var r = new Query
            {
                ExecutionCount = src.ExecutionCount,
                FileName = src.FileName,
                Kind = src.Kind,
                Name = src.Name,
                IsExecutable = src.IsExecutable,
                IsPackaged = src.IsPackaged,
                UniqueIdentifier = src.UniqueIdentifier,
                Icon = src.Icon,
                SearchScore = src.SearchScore,
            };
            return r;
        }

        #endregion Methods
    }
}