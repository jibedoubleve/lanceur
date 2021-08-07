using System.Collections.Generic;

namespace Probel.Lanceur.Core.Entities
{
    public class MultiNameAlias : BaseAlias
    {
        #region Properties

        public IEnumerable<string> Names { get; set; }

        #endregion Properties
    }
}