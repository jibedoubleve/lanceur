using System.Collections.Generic;

namespace Probel.Lanceur.Core.Entities
{
    public class NamedAlias : BaseAlias
    {
        #region Properties

        public IEnumerable<string> Names { get; set; }

        #endregion Properties
    }
}